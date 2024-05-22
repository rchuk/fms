use actix_web::{get, HttpRequest, Responder, web};
use tokio::sync::RwLock;
use crate::application::Application;
use crate::application::auth_manager::{Auth, NoAuth};
use crate::services::ServiceProvider;


pub fn routes() -> actix_web::Scope {
    web::scope("/test")
        .service(test)
}

#[get("")]
pub async fn test(
    req: HttpRequest,
    services: web::Data<RwLock<ServiceProvider>>
) -> impl Responder {
    Application::handle_request(services, req, Auth, || async move {
        Ok(())
    }).await
}
