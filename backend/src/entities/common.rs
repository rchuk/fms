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

macro_rules! define_serial_id {
    ($name:ident) => {
        #[derive(Serialize, Deserialize, PartialEq, Eq, Hash, Debug, Clone, Copy, sqlx::Type)]
        #[sqlx(transparent)]
        #[serde(transparent)]
        pub struct $name(pub i32);

        impl IdType<$name> for $name {
            type Id = i32;

            fn id(self) -> Self::Id {
                self.0
            }
        }
    };
}

macro_rules! define_compound_id2 {
    ($name:ident, $name1:ident: $ty1:ty, $name2:ident: $ty2:ty) => {
        #[derive(Serialize, Deserialize, PartialEq, Eq, Hash, Debug, Clone, Copy, sqlx::FromRow)]
        pub struct $name {
            pub $name1: $ty1,
            pub $name2: $ty2
        }

        impl IdType<$name> for $name {
            type Id = ($ty1, $ty2);

            fn id(self) -> Self::Id {
                (self.$name1, self.$name2)
            }
        }
    };
}

pub(crate) use {define_serial_id, define_compound_id2};
