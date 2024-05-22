use anyhow::Result;
use rust_i18n::t;
use crate::application::session_manager::SessionManager;
use crate::dtos::auth::auth_register_request::AuthRegisterRequestDto;
use crate::errors::public_error::PublicError;


pub struct AuthRegisterRequestValidator;

impl AuthRegisterRequestValidator {
    pub async fn validate(dto: &AuthRegisterRequestDto) -> Result<()> {
        let locale = &SessionManager::locale().await?;

        match dto.username.len() {
            0 => Some(t!("validation.field.email.empty", locale = locale)),
            1..=3 => Some(t!("validation.field.email.short", locale = locale, count = 3)),
            4..=255 => None,
            _ => Some(t!("validation.field.email.long", locale = locale, count = 255))
        }.or_else(|| {
            match dto.password.len() {
                0 => Some(t!("validation.field.password.empty", locale = locale)),
                1..=7 => Some(t!("validation.field.password.short", locale = locale, count = 8)),
                8..=255 => None,
                _ => Some(t!("validation.field.password.long", locale = locale, count = 255))
            }
        }).map_or(Ok(()), |msg|{
            // TODO: Create validation error
            Err(PublicError::client(msg).into())
        })
    }
}
