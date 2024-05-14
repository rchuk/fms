mod models;
mod controllers;
mod converters;
mod mergers;
mod services;
mod entities;
mod repositories;
mod configuration;

use anyhow::Result;
use actix_web::{App, HttpServer};

#[tokio::main]
async fn main() -> Result<()> {
    dotenvy::from_filename("../.env").ok();
    pretty_env_logger::init();

    log::info!("Starting application...");

    let postgres_connect_options = match configuration::postgres::get_postgres_connect_options() {
        Ok(options) => options,
        Err(err) => {
            log::error!("Couldn't get postgres connection options: {}. Caused by: {}", err.to_string(), err.root_cause());

            return Err(err);
        }
    };

    let service_provider = match services::ServiceProvider::new(postgres_connect_options).await {
        Ok(provider) => provider,
        Err(err) => {
            log::error!("Couldn't init services: {}. Caused by: {}", err.to_string(), err.root_cause());

            return Err(err);
        }
    };

    log::info!("Starting http server...");

    HttpServer::new(|| {
        App::new().service(controllers::routes())
    })
    .bind(("0.0.0.0", 3333))?
    .run()
    .await?;

    log::info!("Stopping the application");

    service_provider.stop().await;

    Ok(())
}
