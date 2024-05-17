use actix_web::{get, HttpResponse, Responder, web};


pub fn routes() -> actix_web::Scope {
    web::scope("/test")
        .service(test)
}

#[get("")]
pub async fn test() -> impl Responder {
    HttpResponse::Ok()
}
