use std::fmt::{Display, Formatter};
use serde::Serialize;

#[derive(Serialize, Debug)]
pub struct PublicError {
    #[serde(flatten)]
    kind: PublicErrorKind,
    description: String,
    #[serde(skip_serializing_if = "Option::is_none")]
    dev_description: Option<String>
}

impl PublicError {
    pub fn client(description: impl Into<String>) -> PublicError {
        PublicError {
            kind: PublicErrorKind::Client,
            description: description.into(),
            dev_description: None
        }
    }

    pub fn auth(description: impl Into<String>) -> PublicError {
        PublicError {
            kind: PublicErrorKind::Auth,
            description: description.into(),
            dev_description: None
        }
    }

    pub fn server(description: impl Into<String>) -> PublicError {
        PublicError {
            kind: PublicErrorKind::Server,
            description: description.into(),
            dev_description: None
        }
    }

    pub fn kind(&self) -> &PublicErrorKind {
        &self.kind
    }

    pub fn dev_description(mut self, dev_description: String) -> Self {
        self.dev_description = Some(dev_description);

        self
    }
}

#[derive(Serialize, Debug)]
#[serde(untagged)]
pub enum PublicErrorKind {
    Client,
    Auth,
    Server
}

impl Display for PublicError {
    fn fmt(&self, f: &mut Formatter<'_>) -> std::fmt::Result {
        write!(f, "[PublicError] {}", self.description)?;

        if let Some(dev_description) = &self.dev_description {
            write!(f, ". ({})", dev_description)?;
        }

        Ok(())
    }
}

impl std::error::Error for PublicError {}
