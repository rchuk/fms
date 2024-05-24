use serde::{Deserialize, Serialize};
use crate::entities::common::{define_serial_id, IdType};


#[derive(Debug, sqlx::FromRow)]
pub struct UserEntity<Id: IdType<UserId>> {
    pub id: Id,
    pub username: String,
    pub password_hash: String
}

define_serial_id!(UserId);

