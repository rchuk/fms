use serde::{Deserialize, Serialize};
use crate::entities::common::IdType;


#[derive(Debug, sqlx::FromRow)]
pub struct UserEntity<Id: IdType<UserId>> {
    pub id: Id,
    pub username: String,
    pub password_hash: String
}

#[derive(Serialize, Deserialize, PartialEq, Eq, Hash, Debug, Clone, Copy, sqlx::Type)]
#[sqlx(transparent)]
#[serde(transparent)]
pub struct UserId(pub i32);

impl IdType<UserId> for UserId {
    type Id = i32;

    fn id(self) -> Self::Id {
        self.0
    }
}
