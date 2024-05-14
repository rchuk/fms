use actix_web::{get, HttpResponse, Responder};

#[get("/")]
async fn test() -> impl Responder {
    HttpResponse::Ok()
}
