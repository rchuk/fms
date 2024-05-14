use actix_web::{get, HttpResponse, Responder};

#[get("/test")]
async fn test() -> impl Responder {
    HttpResponse::Ok()
}
