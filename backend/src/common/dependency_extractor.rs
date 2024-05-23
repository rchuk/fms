use std::future::Future;
use std::ops::Deref;
use tokio::join;
use tokio::sync::RwLock;


pub trait DependencyProvider {
    fn provide<T>(&self) -> impl Future<Output = T>
    where
        T: DependencyRetrieverTuple<Self>;

    fn provide_one<T>(&self) -> impl Future<Output = T>
    where
        Self: DependencyExtractor<T>;
}

impl<T> DependencyProvider for T {
    fn provide<DepT>(&self) -> impl Future<Output = DepT>
    where
        DepT: DependencyRetrieverTuple<Self>
    {
        async {
            DepT::retrieve(self).await
            //<T as DependencyExtractor<DepT>>::extract(self).await
        }
    }

    fn provide_one<DepT>(&self) -> impl Future<Output = DepT>
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

/*
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
*/

impl<PtrT, T, DepT> DependencyExtractor<DepT> for PtrT
where
    PtrT: Deref<Target = RwLock<T>>,
    T: DependencyExtractor<DepT>
{
    fn extract(&self) -> impl Future<Output = DepT> {
        async {
            <T as DependencyExtractor<DepT>>::extract(&*self.read().await).await
        }
    }
}

// TODO

pub trait DependencyRetrieverTuple<ExtractorT: ?Sized> {
    fn retrieve(extractor:& ExtractorT) -> impl Future<Output = Self>;
}

impl<ExtractorT> DependencyRetrieverTuple<ExtractorT> for ()
{
    fn retrieve(_extractor: &ExtractorT) -> impl Future<Output = Self> {
        async {
            ()
        }
    }
}

impl<ExtractorT, T1> DependencyRetrieverTuple<ExtractorT> for (T1,)
where
    ExtractorT: DependencyExtractor<T1>
{
    fn retrieve(extractor: &ExtractorT) -> impl Future<Output = Self> {
        async move {
            (extractor.extract().await,)
        }
    }
}

impl<ExtractorT, T1, T2> DependencyRetrieverTuple<ExtractorT> for (T1, T2)
where
    ExtractorT: DependencyExtractor<T1> + DependencyExtractor<T2>
{
    fn retrieve(extractor: &ExtractorT) -> impl Future<Output = Self> {
        async move {
            join!(
                <ExtractorT as DependencyExtractor<T1>>::extract(extractor),
                <ExtractorT as DependencyExtractor<T2>>::extract(extractor)
            )
        }
    }
}

impl<ExtractorT, T1, T2, T3> DependencyRetrieverTuple<ExtractorT> for (T1, T2, T3)
where
    ExtractorT: DependencyExtractor<T1> + DependencyExtractor<T2> + DependencyExtractor<T3>
{
    fn retrieve(extractor: &ExtractorT) -> impl Future<Output = Self> {
        async move {
            join!(
                <ExtractorT as DependencyExtractor<T1>>::extract(extractor),
                <ExtractorT as DependencyExtractor<T2>>::extract(extractor),
                <ExtractorT as DependencyExtractor<T3>>::extract(extractor)
            )
        }
    }
}
