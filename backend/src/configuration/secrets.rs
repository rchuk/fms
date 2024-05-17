use std::collections::HashMap;
use std::num::NonZeroU32;
use anyhow::{Result, Context};


pub struct Secrets {
    pub password_salt: Vec<u8>,
    pub jwt_secret: Vec<u8>,
    pub pbkdf_iterations: NonZeroU32
}

pub fn get_secrets() -> Result<Secrets> {
    use base64::prelude::*;

    let secrets: HashMap<String, String> = dotenvy::from_path_iter("../secrets.env")
        .context("File secrets.env not found. Consider using 'secret_generator' docker container")?
        .flatten()
        .collect();

    let get_env = |name: &str| -> Result<String> {
        secrets.get(name)
            .cloned()
            .with_context(|| format!("Secret environment variable '{name}' is not defined. Consider using 'secret_generator' docker container"))
    };

    let password_salt = BASE64_STANDARD.decode(get_env("PASSWORD_SALT")?)?;
    let jwt_secret = BASE64_STANDARD.decode(get_env("JWT_SECRET")?)?;
    let pbkdf_iterations = (&get_env("PBKDF_ITERATIONS")?).parse()?;

    Ok(Secrets {
        password_salt,
        jwt_secret,
        pbkdf_iterations
    })
}
