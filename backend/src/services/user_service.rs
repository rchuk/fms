use anyhow::Result;
use crate::entities::common::NoId;
use crate::entities::user::{UserEntity, UserId};
use crate::repositories::user_repository::UserRepository;


pub struct UserService {
    repository: UserRepository
}

impl UserService {
    pub fn new(repository: UserRepository) -> Self {
        UserService { repository }
    }

    // TODO: Check permissions (create permission service)
    pub async fn create(&self, entity: UserEntity<NoId>) -> Result<UserId> {
        self.repository.create(entity).await
    }

    pub async fn read_internal(&self, id: UserId) -> Result<Option<UserEntity<UserId>>> {
        self.repository.read(id).await
    }

    pub async fn find_by_email(&self, email: &str) -> Result<Option<UserEntity<UserId>>> {
        self.repository.find_by_email(email.to_owned()).await
    }
}
