use std::future::Future;
use actix_web::{HttpResponse, Responder};
use anyhow::Result;
use rust_i18n::t;
use serde::Serialize;
use crate::errors::public_error::{PublicError, PublicErrorKind};


pub struct ResponseManager {

}

impl ResponseManager {
    pub async fn with_handle_errors<FnT, FutT, ResT>(func: FnT) -> impl Responder
    where
        FnT: FnOnce() -> FutT,
        FutT: Future<Output = Result<ResT>>,
        ResT: Serialize
    {
        match func().await {
            Ok(res) => HttpResponse::Ok().json(res),
            Err(err) => {
                log::error!("{}", err); // Test

                if let Some(err) = err.downcast_ref::<PublicError>() {
                    match err.kind() {
                        PublicErrorKind::Client => HttpResponse::BadRequest(),
                        PublicErrorKind::Auth => HttpResponse::Unauthorized(),
                        PublicErrorKind::Server => HttpResponse::InternalServerError()
                    }.json(err)
                } else {
                    HttpResponse::InternalServerError()
                        .json(PublicError::server(t!("error.unknown-general")))
                }
            }
        }
    }
}
