use std::time::Duration;
use anyhow::{Result, Context};
use sqlx::{Pool, Postgres};
use sqlx::postgres::{PgConnectOptions, PgPoolOptions};

#[derive(Clone)]
pub struct PostgresService {
    pool: Pool<Postgres>
}

impl PostgresService {
    pub async fn new(connect_options: PgConnectOptions) -> Result<Self> {
        let pool = PgPoolOptions::new()
            .max_connections(5)
            .acquire_timeout(Duration::from_secs(5))
            .connect_with(connect_options)
            .await
            .context("Couldn't create database connection pool")?;

        Ok(PostgresService { pool })
    }

    pub async fn stop(self) -> () {
        self.pool.close().await;
    }

    pub async fn run_migrations(&self) -> Result<()> {
        sqlx::migrate!()
            .run(self.pool())
            .await
            .context("Couldn't run database migrations")?;

        Ok(())
    }

    pub fn pool(&self) -> &Pool<Postgres> {
        &self.pool
    }
}
