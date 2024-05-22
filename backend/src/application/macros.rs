
macro_rules! text {
    ($key:expr, $($var_name:tt = $var_val:expr),+ $(,)?) => {
        rust_i18n::t!(
            $key,
            locale = &crate::application::session_manager::SessionManager::locale().await?,
            $($var_name = $var_val),+
        )
    };
    ($key:expr) => {
        rust_i18n::t!(
            $key,
            locale = &crate::application::session_manager::SessionManager::locale().await?
        )
    };
}

macro_rules! lazy_text {
    ($key:expr, $($var_name:tt = $var_val:expr),+ $(,)?) => {
        Box::new(|| Box::pin(async {
            use rust_i18n::t;

            Ok(rust_i18n::t!(
                $key,
                locale = &crate::application::session_manager::SessionManager::locale().await?,
                $($var_name = $var_val),+
            ))
        }) as futures::future::BoxFuture<'static, anyhow::Result<std::borrow::Cow<'static, str>>>)
        as Box<dyn FnOnce() -> futures::future::BoxFuture<'static, anyhow::Result<std::borrow::Cow<'static, str>>>>
    };
    ($key:expr) => {
        Box::new(|| Box::pin(async {
            use rust_i18n::t;

            Ok(rust_i18n::t!(
                $key,
                locale = &crate::application::session_manager::SessionManager::locale().await?,
            ))
        }) as futures::future::BoxFuture<'static, anyhow::Result<std::borrow::Cow<'static, str>>>)
        as Box<dyn FnOnce() -> futures::future::BoxFuture<'static, anyhow::Result<std::borrow::Cow<'static, str>>>>
    };
}

pub(crate) use {text, lazy_text};
