pub mod auth_controller;

use actix_web::web;


pub fn routes() -> actix_web::Scope {
    web::scope("/api")
        .service(auth_controller::routes())
}
