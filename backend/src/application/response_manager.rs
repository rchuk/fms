use std::borrow::Cow;
use std::future::Future;
use actix_web::{HttpResponse, Responder};
use anyhow::Result;
use serde::Serialize;
use crate::errors::public_error::{PublicError, PublicErrorKind};
use crate::text;


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
                    let text = Self::unknown_error_message().await.unwrap_or("Unknown error".into());

                    HttpResponse::InternalServerError().json(PublicError::server(text))
                }
            }
        }
    }

    async fn unknown_error_message() -> Result<Cow<'static, str>> {
        Ok(text!("error.unknown-general"))
    }
}
