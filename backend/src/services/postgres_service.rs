use std::time::Duration;
use anyhow::{Result, Context};
use futures::future::BoxFuture;
use futures::FutureExt;
use sqlx::{PgConnection, Pool, Postgres};
use sqlx::postgres::{PgConnectOptions, PgPoolOptions};
use crate::application::session_manager::SessionManager;


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

    pub async fn stop(&mut self) -> () {
        log::info!("Closing connection pool");

        self.pool.close().await;
    }

    pub async fn begin_transaction(&self) -> Result<()> {
        let transaction_manager = SessionManager::session_data(|data| data.transaction.clone())?;

        transaction_manager.begin(&self.pool).await
    }

    pub async fn commit_transaction(&self) -> Result<()> {
        let transaction_manager = SessionManager::session_data(|data| data.transaction.clone())?;

        transaction_manager.commit().await
    }

    pub async fn rollback_transaction(&self) -> Result<()> {
        let transaction_manager = SessionManager::session_data(|data| data.transaction.clone())?;

        transaction_manager.rollback().await
    }

    pub fn with_conn<FnT, ResT>(&self, func: FnT) -> BoxFuture<'static, Result<ResT>>
    where
        FnT: for<'any> FnOnce(&'any mut PgConnection) -> BoxFuture<'any, Result<ResT, sqlx::Error>> + Send + 'static
    {
        if let Ok(transaction_manager) = SessionManager::session_data(|data| data.transaction.clone()) {
            transaction_manager.with_conn(self.pool.clone(), func)
        } else {
            let pool_clone = self.pool.clone();
            async move {
                let mut conn = pool_clone.acquire().await?;
                let res = func(&mut *conn).await?;

                Ok(res)
            }.boxed()
        }
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
