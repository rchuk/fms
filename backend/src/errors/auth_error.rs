use std::fmt::{Display, Formatter};
use serde::Serialize;

#[derive(Debug, Serialize)]
pub struct AuthError {
    pub description: String,
    pub dev_description: Option<String>
}

impl Display for AuthError {
    fn fmt(&self, f: &mut Formatter<'_>) -> std::fmt::Result {
        write!(f, "[AuthError] {}", self.description)?;

        if let Some(dev_description) = &self.dev_description {
            write!(f, ". ({})", dev_description)?;
        }

        Ok(())
    }
}

impl std::error::Error for AuthError {}
