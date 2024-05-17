use std::fmt::{Display, Formatter};
use serde::Serialize;

// TODO: Create enum (PublicError) and use it as a "base" type for downcasting

#[derive(Debug, Serialize)]
pub struct PublicError {
    pub description: String
}

impl Display for PublicError {
    fn fmt(&self, f: &mut Formatter<'_>) -> std::fmt::Result {
        write!(f, "[PublicError] {}", self.description)
    }
}

impl std::error::Error for PublicError {}
