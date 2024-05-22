use anyhow::Result;
use crate::application::macros::lazy_text;
use crate::dtos::auth::auth_register_request::AuthRegisterRequestDto;
use crate::errors::public_error::PublicError;


pub struct AuthRegisterRequestValidator;

impl AuthRegisterRequestValidator {
    pub async fn validate(dto: &AuthRegisterRequestDto) -> Result<()> {
        let msg = match dto.username.len() {
            0 => Some(lazy_text!("validation.field.email.empty")),
            1..=3 => Some(lazy_text!("validation.field.email.short", count = 3)),
            4..=255 => None,
            _ => Some(lazy_text!("validation.field.email.long", count = 255))
        }.or_else(|| {
            match dto.password.len() {
                0 => Some(lazy_text!("validation.field.password.empty")),
                1..=7 => Some(lazy_text!("validation.field.password.short", count = 8)),
                8..=255 => None,
                _ => Some(lazy_text!("validation.field.password.long", count = 255))
            }
        });

        if let Some(msg) = msg {
            Err(PublicError::client(msg().await?).into())
        } else {
            Ok(())
        }
    }
}
