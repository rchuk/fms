mod dto_validators;
mod dtos;
mod controllers;
mod converters;
mod mergers;
mod services;
mod entities;
mod repositories;
mod configuration;
mod errors;
mod application;
mod common;

use std::sync::Arc;
use anyhow::Result;
use actix_web::{App, HttpServer, middleware, web};
use tokio::sync::RwLock;

rust_i18n::i18n!("locales", fallback = "uk");

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

    let secrets = match configuration::secrets::get_secrets() {
        Ok(secrets) => secrets,
        Err(err) => {
            log::error!("Couldn't get secrets: {}. Caused by: {}", err.to_string(), err.root_cause());

            return Err(err);
        }
    };

    let service_provider = match services::ServiceProvider::new(postgres_connect_options, secrets).await {
        Ok(provider) => Arc::new(RwLock::new(provider)),
        Err(err) => {
            log::error!("Couldn't init services: {}. Caused by: {}", err.to_string(), err.root_cause());

            return Err(err);
        }
    };
    
    let repository_provider = match repositories::RepositoryProvider::new().await {
        Ok(provider) => Arc::new(RwLock::new(provider)),
        Err(err) => {
            log::error!("Couldn't init repositories: {}. Caused by: {}", err.to_string(), err.root_cause());

            return Err(err);
        }
    };

    service_provider.write().await.set_repository_provider(&repository_provider);
    repository_provider.write().await.set_service_provider(&service_provider);

    log::info!("Starting http server...");

    let service_provider_data = web::Data::from(service_provider.clone());
    let repository_provider_data = web::Data::from(repository_provider);

    HttpServer::new(move || {
        App::new()
            .wrap(middleware::Logger::default())
            .app_data(service_provider_data.clone())
            .app_data(repository_provider_data.clone())
            .service(controllers::routes())
    })
    .bind(("0.0.0.0", 3333))?
    .run()
    .await?;

    log::info!("Stopping the application");

    service_provider.write().await.stop().await;

    Ok(())
}
