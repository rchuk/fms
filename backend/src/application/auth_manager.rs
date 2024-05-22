use actix_web::web;
use anyhow::Result;
use async_trait::async_trait;
use rust_i18n::t;
use tokio::sync::RwLock;
use crate::application::session_manager::SessionManager;
use crate::common::dependency_extractor::{DependencyExtractor, DependencyProvider};
use crate::errors::auth_error::AuthError;
use crate::services::auth_service::AuthService;
use crate::services::ServiceProvider;

#[async_trait]
pub trait AuthHandler {
    async fn authorize(&self, token: Option<&str>, provider: web::Data<RwLock<ServiceProvider>>) -> Result<()>
    where ServiceProvider: DependencyExtractor<AuthService>;
}

pub struct NoAuth;
pub struct Auth;

#[async_trait]
impl AuthHandler for NoAuth {
    async fn authorize(&self, _token: Option<&str>, _provider: web::Data<RwLock<ServiceProvider>>) -> Result<()>
    where ServiceProvider: DependencyExtractor<AuthService>
    {
        Ok(())
    }
}

#[async_trait]
impl AuthHandler for Auth {
    async fn authorize(&self, token: Option<&str>, provider: web::Data<RwLock<ServiceProvider>>) -> Result<()>
    where ServiceProvider: DependencyExtractor<AuthService>
    {
        if let Some(token) = token {
            let service: AuthService = provider.into_inner().provide().await;

            if let Some(user_id) = service.get_user_id(token).await? {
                let session_user_id = SessionManager::session_data(|data| data.user_id.clone())?;
                *session_user_id.write().await = Some(user_id);

                return Ok(())
            }

            Err(AuthError {
                description: t!("error.authorization-general").to_string(),
                dev_description: Some("User doesn't exist".to_owned())
            }.into())
        } else {
            Err(AuthError {
                description: t!("error.authorization-general").to_string(),
                dev_description: Some("Missing 'Authorization' header".to_owned())
            }.into())
        }
    }
}
