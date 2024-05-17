use serde::{Deserialize, Deserializer, Serialize};


pub trait IdType<T>: Clone {
    type Id;

    fn id(self) -> Self::Id;
}

#[derive(PartialEq, Eq, Clone, Copy, Default, Serialize, Debug)]
pub struct NoId;

impl<T> IdType<T> for NoId {
    type Id = std::convert::Infallible;

    fn id(self) -> Self::Id {
        unreachable!("You mustn't try to access non-IDs.");
    }
}

impl<'de> Deserialize<'de> for NoId {
    fn deserialize<D>(_deserializer: D) -> Result<Self, D::Error>
    where
        D: Deserializer<'de>,
    {
        Ok(NoId)
    }
}
