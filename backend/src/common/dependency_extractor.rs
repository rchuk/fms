use std::future::Future;
use std::sync::Arc;
use tokio::sync::RwLock;


pub trait DependencyProvider {
    fn provide<T>(&self) -> impl Future<Output = T>
    where
        Self: DependencyExtractor<T>;
}

impl<T> DependencyProvider for T {
    fn provide<DepT>(&self) -> impl Future<Output = DepT>
    where
        Self: DependencyExtractor<DepT>
    {
        async {
            <T as DependencyExtractor<DepT>>::extract(self).await
        }
    }
}

pub trait DependencyExtractor<T> {
    fn extract(&self) -> impl Future<Output = T>;
}

impl<T, DepT> DependencyExtractor<DepT> for Arc<RwLock<T>>
where
    T: DependencyExtractor<DepT>
{
    fn extract(&self) -> impl Future<Output = DepT> {
        async {
            <T as DependencyExtractor<DepT>>::extract(&*self.read().await).await
        }
    }
}
