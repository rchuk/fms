use serde::{Deserialize, Serialize};


#[derive(Debug)]
#[derive(Serialize, Deserialize)]
pub struct AuthPasswordRequestDto {
    pub grant_type: String,
    pub username: String,
    pub password: String
}
