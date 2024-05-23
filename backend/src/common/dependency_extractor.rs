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

pub trait DependencyRetrieverTuple<ExtractorT: ?Sized>: Sized {
    async fn retrieve(extractor: &ExtractorT) -> Self;
}

impl<ExtractorT> DependencyRetrieverTuple<ExtractorT> for ()
{
    async fn retrieve(_extractor: &ExtractorT) -> Self {
        ()
    }
}

macro_rules! tuple_retriever_impl {
    ( $( $name:ident )+ ) => {
        impl<ExtractorT, $($name),+> DependencyRetrieverTuple<ExtractorT> for ($($name,)+)
        where
            ExtractorT: $(DependencyExtractor<$name> + )+
        {
            async fn retrieve(extractor: &ExtractorT) -> Self {
                join!($(<ExtractorT as DependencyExtractor<$name>>::extract(extractor),)+)
            }
        }
    };
}

tuple_retriever_impl!(T1);
tuple_retriever_impl!(T1 T2);
tuple_retriever_impl!(T1 T2 T3);
tuple_retriever_impl!(T1 T2 T3 T4);
tuple_retriever_impl!(T1 T2 T3 T4 T5);
tuple_retriever_impl!(T1 T2 T3 T4 T5 T6);
tuple_retriever_impl!(T1 T2 T3 T4 T5 T6 T7);
tuple_retriever_impl!(T1 T2 T3 T4 T5 T6 T7 T8);
