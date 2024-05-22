use anyhow::Result;
use crate::dtos::auth::auth_password_request::AuthPasswordRequestDto;
use crate::errors::public_error::PublicError;
use crate::{lazy_text};


pub struct AuthPasswordRequestValidator;

impl AuthPasswordRequestValidator {
    pub async fn validate(dto: &AuthPasswordRequestDto) -> Result<()> {
        let msg = match dto.username.len() {
            0 => Some(lazy_text!("validation.field.email.empty")),
            _ => None
        }.or_else(|| {
            match dto.password.len() {
                0 => Some(lazy_text!("validation.field.password.empty")),
                _ => None
            }
        });

        if let Some(msg) = msg {
            Err(PublicError::client(msg().await?).into())
        } else {
            Ok(())
        }
    }
}
