use serde::{Deserialize, Serialize};
use crate::entities::common::{define_serial_id, IdType};


#[derive(Debug, sqlx::FromRow)]
pub struct OrganizationEntity<Id: IdType<OrganizationId>> {
    pub id: Id,
    pub name: String
}

define_serial_id!(OrganizationId);
