pub mod test_controller;

use actix_web::web;

pub fn routes() -> actix_web::Scope {
    web::scope("/api")
        .service( web::scope("/test")
            .service(test_controller::test)
        )
}
