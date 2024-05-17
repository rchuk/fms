use serde::{Deserialize, Serialize};
use crate::entities::user::UserId;


#[derive(Debug, Serialize, Deserialize)]
pub struct JwtClaims {
    pub exp: i64,
    pub iat: i64,
    pub user_id: UserId
}
