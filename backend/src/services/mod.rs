pub mod postgres_service;

use anyhow::{Result, Context};
use sqlx::postgres::PgConnectOptions;
use self::postgres_service::PostgresService;


pub struct ServiceProvider {
    postgres: PostgresService
}

impl ServiceProvider {
    pub async fn new(postgres_connect_options: PgConnectOptions) -> Result<Self> {
        let postgres = PostgresService::new(postgres_connect_options).await?;

        Ok(ServiceProvider {
            postgres
        })
    }

    pub async fn stop(self) {
        self.postgres.stop().await;
    }

    pub fn postgres(&self) -> PostgresService {
        self.postgres.clone()
    }

    // TODO: Other services might be created on demand
}
