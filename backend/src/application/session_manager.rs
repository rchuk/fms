use std::future::Future;
use std::sync::Arc;
use anyhow::Result;
use tokio::sync::RwLock;
use crate::application::transaction_manager::TransactionManager;


pub struct SessionData {
    pub transaction: Arc<TransactionManager>,
    pub locale: Arc<RwLock<String>>
}

tokio::task_local! {
    static SESSION_DATA: SessionData;
}

pub struct SessionManager {

}

impl SessionManager {
    pub async fn scope<ResT>(locale: String, fut: impl Future<Output = ResT>) -> ResT {
        let initial_data = SessionData {
            transaction: TransactionManager::new(),
            locale: Arc::new(RwLock::new(locale))
        };

        SESSION_DATA.scope(
            initial_data,
            fut
        ).await
    }

    pub async fn locale() -> Result<String> {
        let data = Self::session_data(|data| data.locale.clone())?;
        let locale = data.read().await.clone();

        Ok(locale)
    }

    pub fn session_data<ResT, FnT>(func: FnT) -> Result<ResT>
    where
        FnT: FnOnce(&SessionData) -> ResT,
    {
        let res = SESSION_DATA.try_with(func)?;

        Ok(res)
    }
}
