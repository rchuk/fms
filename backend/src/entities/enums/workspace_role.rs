use serde::{Deserialize, Serialize};
use crate::entities::common::{define_serial_id, IdType};

#[derive(Debug, sqlx::FromRow)]
pub struct WorkspaceRoleEntity<Id: IdType<WorkspaceRoleId>> {
    id: Id,
    name: WorkspaceRoleNameEntity
}

#[derive(Serialize, Deserialize, PartialEq, Eq, Hash, Debug, Clone, Copy, sqlx::Type)]
#[sqlx(rename_all = "UPPERCASE")]
pub enum WorkspaceRoleNameEntity {
    Owner,
    Admin,
    Collaborator,
    Viewer
}

define_serial_id!(WorkspaceRoleId);
