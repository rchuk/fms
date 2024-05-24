use serde::{Deserialize, Serialize};
use crate::entities::common::{define_serial_id, IdType};
use crate::entities::enums::workspace_kind::{WorkspaceKindNameEntity};


#[derive(Debug, sqlx::FromRow)]
pub struct WorkspaceEntity<Id: IdType<WorkspaceId>> {
    pub id: Id,
    pub name: String,
    pub kind: WorkspaceKindNameEntity
}

define_serial_id!(WorkspaceId);

