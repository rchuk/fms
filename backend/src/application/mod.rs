pub mod response_manager;
pub mod transaction_manager;
pub mod session_manager;
pub mod auth_manager;

use std::future::Future;
use actix_web::{HttpRequest, Responder, web};
use serde::Serialize;
use tokio::sync::RwLock;
use crate::application::auth_manager::AuthHandler;
use crate::application::response_manager::ResponseManager;
use crate::application::session_manager::SessionManager;
use crate::common::dependency_extractor::DependencyExtractor;
use crate::services::ServiceProvider;


pub struct Application {

}

impl Application {
    pub async fn handle_request_with_service<ServiceT, FnT, FutT, ResT, AuthHandlerT>
        (provider: web::Data<RwLock<ServiceProvider>>, req: HttpRequest, auth: AuthHandlerT, func: FnT) -> impl Responder
    where
        ServiceProvider: DependencyExtractor<ServiceT>,
        FnT: FnOnce(ServiceT) -> FutT,
        FutT: Future<Output = anyhow::Result<ResT>>,
        ResT: Serialize,
        AuthHandlerT: AuthHandler
    {
        Self::handle_request(provider.clone(), req, auth, || async move {
            func(provider.extract().await).await
        }).await
    }

    pub async fn handle_request<FnT, FutT, ResT, AuthHandlerT>
        (provider: web::Data<RwLock<ServiceProvider>>, req: HttpRequest, auth: AuthHandlerT, func: FnT) -> impl Responder
    where
        FnT: FnOnce() -> FutT,
        FutT: Future<Output = anyhow::Result<ResT>>,
        ResT: Serialize,
        AuthHandlerT: AuthHandler
    {
        // TODO: Provide configuration for default locale
        let locale = Self::get_header(&req, "Accept-Language").unwrap_or("uk".to_owned());
        let authorization = Self::get_header(&req, "Authorization")
            .and_then(|value| value.strip_prefix("Bearer ").map(ToOwned::to_owned)); //TODO

        SessionManager::scope(locale.to_string(), async move {
            ResponseManager::with_handle_errors(move || async move {
                auth.authorize(authorization.as_deref(), provider).await?;

                func().await
            }).await
        }).await
    }

    fn get_header(req: &HttpRequest, name: &str) -> Option<String> {
        req.headers()
            .get(name)
            .map(|value| value.to_str().ok())
            .flatten()
            .map(ToOwned::to_owned)
    }
}
