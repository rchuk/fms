use serde::{Deserialize, Serialize};
use crate::entities::account::AccountId;
use crate::entities::common::{define_compound_id2, IdType};
use crate::entities::enums::workspace_role::WorkspaceRoleId;
use crate::entities::workspace::WorkspaceId;


#[derive(Debug, sqlx::FromRow)]
pub struct WorkspaceToAccountEntity<Id: IdType<WorkspaceToAccountId>> {
    pub id: Id,
    pub role: WorkspaceRoleId
}

define_compound_id2!(WorkspaceToAccountId, workspace_id: WorkspaceId, account_id: AccountId);
