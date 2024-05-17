pub mod test_controller;
pub mod auth_controller;

use actix_web::web;


pub fn routes() -> actix_web::Scope {
    web::scope("/api")
        .service(test_controller::routes())
        .service(auth_controller::routes())
}
