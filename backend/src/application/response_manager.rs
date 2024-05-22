use std::future::Future;
use actix_web::{HttpResponse, Responder};
use anyhow::Result;
use serde::Serialize;
use crate::errors::auth_error::AuthError;
use crate::errors::client_error::ClientError;
use crate::errors::public_error::PublicError;


pub struct ResponseManager {

}

impl ResponseManager {
    pub async fn with_handle_errors<FnT, FutT, ResT>(func: FnT) -> impl Responder
    where
        FnT: FnOnce() -> FutT,
        FutT: Future<Output = Result<ResT>>,
        ResT: Serialize
    {
        // TODO: Refactor errors to be a enum (PublicError) to use only one downcast
        match func().await {
            Ok(res) => HttpResponse::Ok().json(res),
            Err(err) => {
                log::error!("{}", err); // Test

                if let Some(err) = err.downcast_ref::<PublicError>() {
                    HttpResponse::InternalServerError().json(err)
                } else if let Some(err) = err.downcast_ref::<ClientError>() {
                    HttpResponse::BadRequest().json(err)
                } else if let Some(err) = err.downcast_ref::<AuthError>() {
                    HttpResponse::Unauthorized().json(err)
                } else {
                    HttpResponse::InternalServerError().finish()
                }
            }
        }
    }
}
