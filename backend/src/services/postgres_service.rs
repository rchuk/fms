use std::time::Duration;
use anyhow::{Result, Context};
use sqlx::{ConnectOptions, Pool, Postgres};
use sqlx::postgres::{PgConnectOptions, PgPoolOptions};

#[derive(Clone)]
pub struct PostgresService {
    pool: Pool<Postgres>
}

impl PostgresService {
    pub async fn new(connect_options: PgConnectOptions) -> Result<Self> {
        log::info!("Acquiring database connection");

        let pool = PgPoolOptions::new()
            .max_connections(5)
            .acquire_timeout(Duration::from_secs(5))
            .connect_with(connect_options)
            .await
            .context("Couldn't create database connection pool")?;

        log::info!("Successfully acquired database connection");

        Self::run_migrations(&pool).await?;

        Ok(PostgresService { pool })
    }

    pub async fn stop(self) -> () {
        log::info!("Closing connection pool");

        self.pool.close().await;
    }

    pub fn pool(&self) -> &Pool<Postgres> {
        &self.pool
    }

    async fn run_migrations(pool: &Pool<Postgres>) -> Result<()> {
        log::info!("Running database migrations...");

        sqlx::migrate!()
            .run(pool)
            .await
            .context("Couldn't run database migrations")?;

        log::info!("Successfully completed database migrations");

        Ok(())
    }
}
