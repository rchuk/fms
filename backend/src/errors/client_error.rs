use std::fmt::{Display, Formatter};
use serde::Serialize;

#[derive(Debug, Serialize)]
pub struct ClientError {
    pub description: String
}

impl Display for ClientError {
    fn fmt(&self, f: &mut Formatter<'_>) -> std::fmt::Result {
        write!(f, "[ClientError] {}", self.description)
    }
}

impl std::error::Error for ClientError {}
