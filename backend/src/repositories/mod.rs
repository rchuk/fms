pub mod user_repository;

use std::future::Future;
use std::sync::{Arc, Weak};
use anyhow::Result;
use tokio::sync::RwLock;
use crate::common::dependency_extractor::DependencyExtractor;
use crate::repositories::user_repository::UserRepository;
use crate::services::ServiceProvider;


pub struct RepositoryProvider {
    service_provider: Weak<RwLock<ServiceProvider>>
}

impl RepositoryProvider {
    pub async fn new() -> Result<Self> {
        Ok(RepositoryProvider {
            service_provider: Weak::new()
        })
    }

    pub async fn user_repository(&self) -> UserRepository {
        let db = self.service_provider().extract().await;

        UserRepository::new(db)
    }

    pub fn set_service_provider(&mut self, service_provider: &Arc<RwLock<ServiceProvider>>) {
        self.service_provider = Arc::downgrade(service_provider);
    }

    fn service_provider(&self) -> Arc<RwLock<ServiceProvider>> {
        self.service_provider.upgrade().expect("Service provider is not initialized")
    }
}

impl DependencyExtractor<UserRepository> for RepositoryProvider {
    fn extract(&self) -> impl Future<Output = UserRepository> {
        self.user_repository()
    }
}
