use std::sync::Arc;
use anyhow::Result;
use chrono::{Duration, Utc};
use rust_i18n::t;
use crate::application::session_manager::SessionManager;
use crate::configuration::secrets::Secrets;
use crate::dto_validators::auth::auth_password_request_validator::AuthPasswordRequestValidator;
use crate::dto_validators::auth::auth_register_request_validator::AuthRegisterRequestValidator;
use crate::dtos::auth::auth_password_request::AuthPasswordRequestDto;
use crate::dtos::auth::auth_register_request::AuthRegisterRequestDto;
use crate::dtos::auth::auth_token::AuthTokenDto;
use crate::entities::json_web_token::JwtClaims;
use crate::entities::common::NoId;
use crate::entities::user::{UserEntity, UserId};
use crate::errors::client_error::ClientError;
use crate::services::postgres_service::PostgresService;
use crate::services::user_service::UserService;

static PBKDF2_ALG: ring::pbkdf2::Algorithm = ring::pbkdf2::PBKDF2_HMAC_SHA256;
const CREDENTIAL_LEN: usize = ring::digest::SHA256_OUTPUT_LEN;
pub type Credential = [u8; CREDENTIAL_LEN];


// TODO: Decouple JWT tokens from password management
pub struct AuthService {
    db: PostgresService,
    user_service: UserService,
    secrets: Arc<Secrets>
}

impl AuthService {
    pub fn new(db: PostgresService, user_service: UserService, secrets: Arc<Secrets>) -> Self {
        AuthService { db, user_service, secrets }
    }

    pub async fn register(&self, auth_register_request_dto: AuthRegisterRequestDto) -> Result<AuthTokenDto> {
        AuthRegisterRequestValidator::validate(&auth_register_request_dto).await?;

        self.db.begin_transaction().await?;

        if let Some(_) = self.user_service.find_by_email(&auth_register_request_dto.username).await? {
            return Err(ClientError {
                description: t!("error.user-already-exists-by-email", locale = &SessionManager::locale().await?).to_string()
            }.into());
        }

        let password_hash = self.hash_password(&auth_register_request_dto.username, &auth_register_request_dto.password)?;
        let user_id = self.user_service.create(UserEntity {
            id: NoId,
            username: auth_register_request_dto.username,
            password_hash
        }).await?;

        let access_token = self.create_token(user_id)?;

        self.db.commit_transaction().await?;

        Ok(self.build_auth_token_dto(access_token))
    }

    pub async fn get_token(&self, auth_password_request_dto: AuthPasswordRequestDto) -> Result<AuthTokenDto> {
        AuthPasswordRequestValidator::validate(&auth_password_request_dto).await?;

        let access_token = if let Some(user) = self.user_service.find_by_email(&auth_password_request_dto.username).await? {
            if !self.verify_password(&user, &auth_password_request_dto.password) {
                return Err(ClientError {
                    description: t!("error.wrong-password", locale = &SessionManager::locale().await?).to_string()
                }.into());
            }

            self.create_token(user.id)?
        } else {
            return Err(ClientError {
                description: t!("error.user-doesnt-exist-by-email", locale = &SessionManager::locale().await?).to_string()
            }.into());
        };

        Ok(self.build_auth_token_dto(access_token))
    }

    pub async fn get_user_id(&self, token: &str) -> Result<Option<UserId>> {
        if let Ok(claims) = self.decode_token(token) {
            Ok(Some(claims.user_id))
        } else {
            Ok(None)
        }
    }

    fn build_auth_token_dto(&self, access_token: String) -> AuthTokenDto {
        AuthTokenDto {
            access_token,
            token_type: "Bearer".to_string(),
            expires_in: self.expiration_time().num_seconds() as usize
        }
    }

    fn create_token(&self, user_id: UserId) -> Result<String> {
        use jsonwebtoken::{Header, EncodingKey};

        let issued_at = Utc::now();
        let expires_at = issued_at + self.expiration_time();
        let claims = JwtClaims {
            iat: issued_at.timestamp(),
            exp: expires_at.timestamp(),
            user_id
        };

        let secret = &self.secrets.jwt_secret;
        let token = jsonwebtoken::encode(&Header::default(), &claims, &EncodingKey::from_secret(secret))?;

        Ok(token)
    }

    fn decode_token(&self, token: &str) -> Result<JwtClaims> {
        use jsonwebtoken::{DecodingKey, Validation};

        let secret = &self.secrets.jwt_secret;
        let data = jsonwebtoken::decode::<JwtClaims>(token, &DecodingKey::from_secret(secret), &Validation::default())?;

        Ok(data.claims)
    }

    fn expiration_time(&self) -> Duration {
        // TODO: Provide configuration
        Duration::hours(1)
    }

    fn verify_password(&self, user: &UserEntity<UserId>, attempted_password: &str) -> bool {
        use ring::pbkdf2;
        use base64::prelude::*;

        if let Ok(actual_password_hash) = BASE64_STANDARD.decode(&user.password_hash) {
            let salt = self.salt_password(&user.username);

            pbkdf2::verify(PBKDF2_ALG, self.secrets.pbkdf_iterations.into(), &salt, attempted_password.as_bytes(), &actual_password_hash).is_ok()
        } else {
            false
        }
    }

    fn hash_password(&self, username: &str, password: &str) -> Result<String> {
        use ring::pbkdf2;
        use base64::prelude::*;

        let salt = self.salt_password(username);
        let mut credential: Credential = [0u8; CREDENTIAL_LEN];
        pbkdf2::derive(PBKDF2_ALG, self.secrets.pbkdf_iterations.into(), &salt, password.as_bytes(), &mut credential);
        let hash = BASE64_STANDARD.encode(credential);

        Ok(hash)
    }

    fn salt_password(&self, username: &str) -> Vec<u8> {
        let password_salt = &self.secrets.password_salt;

        let mut salt = Vec::with_capacity(password_salt.len() + username.as_bytes().len());
        salt.extend(password_salt);
        salt.extend(username.as_bytes());

        salt
    }
}
