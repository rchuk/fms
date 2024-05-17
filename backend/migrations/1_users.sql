CREATE TABLE users
(
    id              SERIAL PRIMARY KEY,
    username        VARCHAR(255) UNIQUE,
    password_hash   VARCHAR(344)
);
