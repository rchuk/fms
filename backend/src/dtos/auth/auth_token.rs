use serde::{Serialize, Deserialize};


#[derive(Debug)]
#[derive(Serialize, Deserialize)]
pub struct AuthTokenDto {
    pub access_token: String,
    pub token_type: String,
    pub expires_in: usize
}
