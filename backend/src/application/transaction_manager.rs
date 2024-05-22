use std::sync::Arc;
use anyhow::{anyhow, Result};
use futures::future::BoxFuture;
use futures::FutureExt;
use sqlx::{PgConnection, Pool, Postgres};
use tokio::sync::Mutex;


pub struct TransactionManager {
    transaction: Mutex<Option<sqlx::Transaction<'static, Postgres>>>
}

impl TransactionManager {
    pub fn new() -> Arc<Self> {
        Arc::new(TransactionManager {
            transaction: Mutex::new(None)
        })
    }

    pub fn with_conn<FnT, ResT>(self: Arc<Self>, fallback: Pool<Postgres>, func: FnT) -> BoxFuture<'static, Result<ResT>>
    where
        FnT: for<'any> FnOnce(&'any mut PgConnection) -> BoxFuture<'any, Result<ResT, sqlx::Error>> + Send + 'static
    {
        let self_clone = self.clone();
        async move {
            let mut guard = self_clone.transaction.lock().await;
            let res = if let Some(transaction) = &mut *guard {
                func(&mut *transaction).await?
            } else {
                let mut pool_conn = fallback.acquire().await?;
                func(&mut *pool_conn).await?
            };

            Ok(res)
        }.boxed()
    }

    pub async fn is_in_progress(&self) -> bool {
        self.transaction.lock().await.is_some()
    }

    pub async fn begin(&self, pool: &Pool<Postgres>) -> Result<()> {
        if self.is_in_progress().await {
            return Ok(()); // NOTE: Transactions are not nested
        }

        let transaction = pool.begin().await?;

        *self.transaction.lock().await = Some(transaction);

        Ok(())
    }

    pub async fn commit(&self) -> Result<()> {
        let mut guard = self.transaction.lock().await;
        if let Some(transaction) = guard.take() {
            transaction.commit().await?;

            Ok(())
        } else {
            Err(anyhow!("Tried to commit outside of any transaction"))
        }
    }

    pub async fn rollback(&self) -> Result<()> {
        let mut guard = self.transaction.lock().await;
        if let Some(transaction) = guard.take() {
            transaction.rollback().await?;
        }

        log::warn!("Using rollback outside of a transaction");

        Ok(())
    }
}
