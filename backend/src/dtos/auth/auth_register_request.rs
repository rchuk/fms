use serde::{Deserialize, Serialize};


#[derive(Debug)]
#[derive(Serialize, Deserialize)]
pub struct AuthRegisterRequestDto {
    pub username: String,
    pub password: String
}
