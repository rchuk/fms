use anyhow::Result;
use futures::FutureExt;
use crate::entities::common::NoId;
use crate::entities::user::{UserEntity, UserId};
use crate::services::postgres_service::PostgresService;


pub struct UserRepository {
    db: PostgresService
}

impl UserRepository {
    pub fn new(db: PostgresService) -> Self {
        UserRepository { db }
    }

    pub async fn create(&self, user_entity: UserEntity<NoId>) -> Result<UserId> {
        let (id,): (UserId,) = self.db.with_conn(|conn| {
            sqlx::query_as(
                r#"
                INSERT INTO users(username, password_hash)
                VALUES ($1, $2)
                RETURNING id;
                "#
            )
            .bind(user_entity.username)
            .bind(user_entity.password_hash)
            .fetch_one(conn)
        }.boxed()).await?;

        Ok(id)
    }

    pub async fn read(&self, id: UserId) -> Result<Option<UserEntity<UserId>>> {
        let user: Option<UserEntity<UserId>> = self.db.with_conn(move |conn| {
            sqlx::query_as(
                r#"
                SELECT * FROM users
                WHERE id = $1;
                "#
            )
            .bind(id)
            .fetch_optional(conn)
        }.boxed()).await?;

        Ok(user)
    }

    pub async fn update(&self, user_entity: UserEntity<UserId>) -> Result<bool> {
        let result = self.db.with_conn(move |conn| {
            sqlx::query(
                r#"
                UPDATE users
                SET
                username = $2
                password_hash = $3
                WHERE id = $1;
                "#
            )
            .bind(user_entity.id)
            .bind(user_entity.username)
            .bind(user_entity.password_hash)
            .execute(conn)
        }.boxed()).await?;

        Ok(result.rows_affected() != 0)
    }

    pub async fn delete(&self, id: UserId) -> Result<bool> {
        let result = self.db.with_conn(move |conn| {
            sqlx::query(
                r#"
                DELETE FROM users
                WHERE id = $1;
                "#
            )
            .bind(id)
            .execute(conn)
        }.boxed()).await?;

        Ok(result.rows_affected() != 0)
    }

    pub async fn find_by_email(&self, email: String) -> Result<Option<UserEntity<UserId>>> {
        let user: Option<UserEntity<UserId>> = self.db.with_conn(move |conn| {
            sqlx::query_as(
                r#"
                SELECT * FROM users
                WHERE username = $1;
                "#
            )
            .bind(email)
            .fetch_optional(conn)
        }.boxed()).await?;

        Ok(user)
    }
}
