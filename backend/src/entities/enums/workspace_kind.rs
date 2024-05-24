use serde::{Deserialize, Serialize};
use crate::entities::common::{define_serial_id, IdType};

#[derive(Debug, sqlx::FromRow)]
pub struct WorkspaceKindEntity<Id: IdType<WorkspaceKindId>> {
    id: Id,
    name: WorkspaceKindNameEntity
}

#[derive(Serialize, Deserialize, PartialEq, Eq, Hash, Debug, Clone, Copy, sqlx::Type)]
#[sqlx(rename_all = "UPPERCASE")]
pub enum WorkspaceKindNameEntity {
    Personal,
    Shared
}

define_serial_id!(WorkspaceKindId);


