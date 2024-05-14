mod models;
mod controllers;
mod converters;
mod mergers;
mod services;
mod entities;
mod repositories;

use std::env;
use anyhow::{Context, Result};
use actix_web::{App, HttpServer, web};
use sqlx::postgres::PgConnectOptions;
use crate::services::postgres_service::PostgresService;
use crate::controllers::test_controller;

fn get_postgres_connect_options() -> Result<PgConnectOptions> {
    fn get_env(name: &str) -> Result<String> {
        env::var(name).with_context(|| format!("Environment variable '{name}' is not defined"))
    }

    Ok(PgConnectOptions::new()
        .host(&get_env("POSTGRES_HOST")?)
        .port((&get_env("POSTGRES_PORT")?).parse()?)
        .database(&get_env("POSTGRES_NAME")?)
        .username(&get_env("POSTGRES_USER")?)
        .password(&get_env("POSTGRES_PASSWORD")?)
    )
}

#[tokio::main]
async fn main() -> Result<()> {
    println!("Starting server..."); // TODO: Add logging

    let _ = dotenvy::from_filename("../.env");

    let postgres_service = PostgresService::new(get_postgres_connect_options()?).await?;
    postgres_service.run_migrations().await?;

    HttpServer::new(|| {
        App::new()
            .service(web::scope("/api")
                .service(test_controller::test)
            )
    })
    .bind(("0.0.0.0", 3333))?
    .run()
    .await?;

    postgres_service.stop().await;

    Ok(())
}
