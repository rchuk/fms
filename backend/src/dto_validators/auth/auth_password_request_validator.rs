use anyhow::Result;
use rust_i18n::t;
use crate::application::session_manager::SessionManager;
use crate::dtos::auth::auth_password_request::AuthPasswordRequestDto;
use crate::errors::public_error::PublicError;


pub struct AuthPasswordRequestValidator;

impl AuthPasswordRequestValidator {
    pub async fn validate(dto: &AuthPasswordRequestDto) -> Result<()> {
        let locale = &SessionManager::locale().await?;

        match dto.username.len() {
            0 => Some(t!("validation.field.email.empty", locale = locale)),
            _ => None
        }.or_else(|| {
            match dto.password.len() {
                0 => Some(t!("validation.field.password.empty", locale = locale)),
                _ => None
            }
        }).map_or(Ok(()), |msg|{
            // TODO: Create validation error
            Err(PublicError::client(msg).into())
        })
    }
}
