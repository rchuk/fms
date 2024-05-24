use anyhow::Result;
use futures::FutureExt;
use crate::entities::common::NoId;
use crate::entities::workspace::{WorkspaceEntity, WorkspaceId};
use crate::services::postgres_service::PostgresService;


pub struct WorkspaceRepository {
    db: PostgresService
}

impl WorkspaceRepository {
    pub fn new((db,): (PostgresService,)) -> Self {
        WorkspaceRepository { db }
    }

    pub async fn create(&self, entity: WorkspaceEntity<NoId>) -> Result<WorkspaceId> {
        let (id,): (WorkspaceId,) = self.db.with_conn(move |conn| {
            sqlx::query_as(
                r#"
                INSERT INTO workspaces(name, kind)
                VALUES (
                    $1,
                    (SELECT id FROM workspace_kinds WHERE name = $2)
                )
                RETURNING id;
                "#
            )
            .bind(entity.name)
            .bind(entity.kind)
            .fetch_one(conn)
        }.boxed()).await?;

        Ok(id)
    }

    pub async fn read(&self, id: WorkspaceId) -> Result<Option<WorkspaceEntity<WorkspaceId>>> {
        let user: Option<WorkspaceEntity<WorkspaceId>> = self.db.with_conn(move |conn| {
            sqlx::query_as(
                r#"
                SELECT * FROM workspaces
                WHERE id = $1;
                "#
            )
            .bind(id)
            .fetch_optional(conn)
        }.boxed()).await?;

        Ok(user)
    }

    pub async fn update(&self, entity: WorkspaceEntity<WorkspaceId>) -> Result<bool> {
        let result = self.db.with_conn(move |conn| {
            sqlx::query(
                r#"
                UPDATE workspaces
                SET
                name = $2
                kind = $3
                WHERE id = $1;
                "#
            )
            .bind(entity.id)
            .bind(entity.name)
            .bind(entity.kind)
            .execute(conn)
        }.boxed()).await?;

        Ok(result.rows_affected() != 0)
    }

    pub async fn delete(&self, id: WorkspaceId) -> Result<bool> {
        let result = self.db.with_conn(move |conn| {
            sqlx::query(
                r#"
                DELETE FROM workspaces
                WHERE id = $1;
                "#
            )
            .bind(id)
            .execute(conn)
        }.boxed()).await?;

        Ok(result.rows_affected() != 0)
    }
}
