pub mod response_manager;
pub mod transaction_manager;
pub mod session_manager;

use std::future::Future;
use actix_web::{HttpRequest, Responder, web};
use serde::Serialize;
use tokio::sync::RwLock;
use crate::application::response_manager::ResponseManager;
use crate::application::session_manager::SessionManager;
use crate::common::dependency_extractor::DependencyExtractor;
use crate::services::ServiceProvider;


pub struct Application {

}

impl Application {
    pub async fn handle_request_with_service<ServiceT, FnT, FutT, ResT>
        (provider: web::Data<RwLock<ServiceProvider>>, req: &HttpRequest, func: FnT) -> impl Responder
    where
        ServiceProvider: DependencyExtractor<ServiceT>,
        FnT: FnOnce(ServiceT) -> FutT,
        FutT: Future<Output = anyhow::Result<ResT>>,
        ResT: Serialize,
    {
        Self::handle_request(req, || async move {
            func(provider.extract().await).await
        }).await
    }

    pub async fn handle_request<FnT, FutT, ResT>(req: &HttpRequest, func: FnT) -> impl Responder
    where
        FnT: FnOnce() -> FutT,
        FutT: Future<Output = anyhow::Result<ResT>>,
        ResT: Serialize
    {
        // TODO: Provide configuration for default locale
        let locale = req.headers()
            .get("Accept-Language")
            .map(|value| value.to_str().ok())
            .flatten()
            .unwrap_or("uk")
            .to_string();

        SessionManager::scope(locale, async move {
            ResponseManager::with_handle_errors(|| {
                func()
            }).await
        }).await
    }
}
