pub mod auth_service;
pub mod user_service;
pub mod postgres_service;

use std::future::Future;
use std::sync::{Arc, Weak};
use anyhow::Result;
use sqlx::postgres::PgConnectOptions;
use tokio::sync::RwLock;
use crate::common::dependency_extractor::{DependencyExtractor, DependencyProvider};
use crate::configuration::secrets::Secrets;
use crate::repositories::RepositoryProvider;
use crate::services::auth_service::AuthService;
use crate::services::user_service::UserService;
use self::postgres_service::PostgresService;


#[derive(Clone)]
pub struct ServiceProvider {
    repository_provider: Weak<RwLock<RepositoryProvider>>,

    postgres: PostgresService,
    secrets: Arc<Secrets>
}

impl ServiceProvider {
    pub async fn new(postgres_connect_options: PgConnectOptions, secrets: Secrets) -> Result<Self> {
        let postgres = PostgresService::new(postgres_connect_options).await?;

        Ok(ServiceProvider {
            repository_provider: Weak::new(),
            postgres,
            secrets: Arc::new(secrets)
        })
    }

    pub async fn auth_service(&self) -> AuthService {
        // TODO: Create dependency injection for tuples
        AuthService::new(self.provide().await, self.provide().await, self.provide().await)
    }

    pub async fn user_service(&self) -> UserService {
        let user_repository = self.repository_provider().extract().await;

        UserService::new(user_repository)
    }

    pub async fn postgres(&self) -> PostgresService {
        self.postgres.clone()
    }

    pub async fn secrets(&self) -> Arc<Secrets> {
        self.secrets.clone()
    }

    pub fn set_repository_provider(&mut self, repository_provider: &Arc<RwLock<RepositoryProvider>>) {
        self.repository_provider = Arc::downgrade(repository_provider);
    }

    pub async fn stop(&mut self) {
        self.postgres.stop().await;
    }

    fn repository_provider(&self) -> Arc<RwLock<RepositoryProvider>> {
        self.repository_provider.upgrade().expect("Repository provider is not initialized")
    }
}

impl DependencyExtractor<AuthService> for ServiceProvider {
    fn extract(&self) -> impl Future<Output = AuthService> {
        self.auth_service()
    }
}

impl DependencyExtractor<PostgresService> for ServiceProvider {
    fn extract(&self) -> impl Future<Output = PostgresService> {
        self.postgres()
    }
}

impl DependencyExtractor<UserService> for ServiceProvider {
    fn extract(&self) -> impl Future<Output = UserService> {
        self.user_service()
    }
}

impl DependencyExtractor<Arc<Secrets>> for ServiceProvider {
    fn extract(&self) -> impl Future<Output = Arc<Secrets>> {
        self.secrets()
    }
}
