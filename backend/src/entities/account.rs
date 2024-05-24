use serde::{Deserialize, Serialize};
use crate::entities::common::{define_serial_id, IdType};
use crate::entities::organization::OrganizationId;
use crate::entities::user::UserId;


#[derive(Debug, sqlx::FromRow)]
pub struct AccountEntity<Id: IdType<AccountId>> {
    pub id: Id,
    pub user_id: Option<UserId>,
    pub organization_id: Option<OrganizationId>
}

define_serial_id!(AccountId);
