use serde::{Deserialize, Serialize};
use crate::entities::common::{define_serial_id, IdType};

#[derive(Debug, sqlx::FromRow)]
pub struct OrganizationRoleEntity<Id: IdType<OrganizationRoleId>> {
    id: Id,
    name: OrganizationRoleNameEntity
}

#[derive(Serialize, Deserialize, PartialEq, Eq, Hash, Debug, Clone, Copy, sqlx::Type)]
#[sqlx(rename_all = "UPPERCASE")]
pub enum OrganizationRoleNameEntity {
    Owner,
    Admin,
    Member
}

define_serial_id!(OrganizationRoleId);
