use actix_web::{get, post, HttpResponse, Responder, web, HttpRequest};
use tokio::sync::RwLock;
use crate::application::Application;
use crate::application::auth_manager::NoAuth;
use crate::dtos::auth::auth_password_request::AuthPasswordRequestDto;
use crate::dtos::auth::auth_register_request::AuthRegisterRequestDto;
use crate::services::auth_service::AuthService;
use crate::services::ServiceProvider;


pub fn routes() -> actix_web::Scope {
    web::scope("/auth")
        .service(login)
        .service(token)
        .service(register)
}

#[get("/login")]
pub async fn login() -> impl Responder {
    // TODO: Implement
    HttpResponse::InternalServerError()
}

#[post("/token")]
pub async fn token(
    req: HttpRequest,
    web::Form(body): web::Form<AuthPasswordRequestDto>,
    services: web::Data<RwLock<ServiceProvider>>
) -> impl Responder {
    Application::handle_request_with_service(services, req, NoAuth, |service: AuthService| async move {
        service.get_token(body).await
    }).await
}

#[post("/register")]
pub async fn register(
    req: HttpRequest,
    web::Json(body): web::Json<AuthRegisterRequestDto>,
    services: web::Data<RwLock<ServiceProvider>>
) -> impl Responder {
    Application::handle_request_with_service(services, req, NoAuth, |service: AuthService| async move {
        service.register(body).await
    }).await
}
