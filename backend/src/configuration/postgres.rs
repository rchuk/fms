use std::env;
use anyhow::{Result, Context};
use log::LevelFilter;
use sqlx::ConnectOptions;
use sqlx::postgres::PgConnectOptions;

pub fn get_postgres_connect_options() -> Result<PgConnectOptions> {
    fn get_env(name: &str) -> Result<String> {
        env::var(name).with_context(|| format!("Environment variable '{name}' is not defined"))
    }

    let log_level = env::var("SQL_LOG")
        .ok()
        .and_then(|level| level.parse().ok())
        .unwrap_or(LevelFilter::Off);

    Ok(PgConnectOptions::new()
        .host(&get_env("POSTGRES_HOST")?)
        .port((&get_env("POSTGRES_PORT")?).parse()?)
        .database(&get_env("POSTGRES_NAME")?)
        .username(&get_env("POSTGRES_USER")?)
        .password(&get_env("POSTGRES_PASSWORD")?)
        .log_statements(log_level)
    )
}
