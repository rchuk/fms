use std::ops::Deref;
use tokio::join;
use tokio::sync::RwLock;


pub trait DependencyProvider {
    async fn provide<T>(&self) -> T
    where
        T: DependencyRetrieverTuple<Self>;

    async fn provide_one<T>(&self) -> T
    where
        Self: DependencyExtractor<T>;
}

impl<T> DependencyProvider for T {
    async fn provide<DepT>(&self) -> DepT
    where
        DepT: DependencyRetrieverTuple<Self>
    {
        DepT::retrieve(self).await
    }

    async fn provide_one<DepT>(&self) -> DepT
    where
        Self: DependencyExtractor<DepT>
    {
        <T as DependencyExtractor<DepT>>::extract(self).await
    }
}

pub trait DependencyExtractor<T> {
    async fn extract(&self) -> T;
}

impl<PtrT, T, DepT> DependencyExtractor<DepT> for PtrT
where
    PtrT: Deref<Target = RwLock<T>>,
    T: DependencyExtractor<DepT>
{
    async fn extract(&self) -> DepT {
        <T as DependencyExtractor<DepT>>::extract(&*self.read().await).await
    }
}

// TODO: Create implementation for all tuples using macros

pub trait DependencyRetrieverTuple<ExtractorT: ?Sized>: Sized {
    async fn retrieve(extractor: &ExtractorT) -> Self;
}

impl<ExtractorT> DependencyRetrieverTuple<ExtractorT> for ()
{
    async fn retrieve(_extractor: &ExtractorT) -> Self {
        ()
    }
}

impl<ExtractorT, T1> DependencyRetrieverTuple<ExtractorT> for (T1,)
where
    ExtractorT: DependencyExtractor<T1>
{
    async fn retrieve(extractor: &ExtractorT) -> Self {
        (extractor.extract().await,)
    }
}

impl<ExtractorT, T1, T2> DependencyRetrieverTuple<ExtractorT> for (T1, T2)
where
    ExtractorT: DependencyExtractor<T1> + DependencyExtractor<T2>
{
    async fn retrieve(extractor: &ExtractorT) -> Self {
        join!(
            <ExtractorT as DependencyExtractor<T1>>::extract(extractor),
            <ExtractorT as DependencyExtractor<T2>>::extract(extractor)
        )
    }
}

impl<ExtractorT, T1, T2, T3> DependencyRetrieverTuple<ExtractorT> for (T1, T2, T3)
where
    ExtractorT: DependencyExtractor<T1> + DependencyExtractor<T2> + DependencyExtractor<T3>
{
    async fn retrieve(extractor: &ExtractorT) -> Self {
        join!(
            <ExtractorT as DependencyExtractor<T1>>::extract(extractor),
            <ExtractorT as DependencyExtractor<T2>>::extract(extractor),
            <ExtractorT as DependencyExtractor<T3>>::extract(extractor)
        )
    }
}
