ALTER TABLE users
    ADD COLUMN first_name VARCHAR(255) NOT NULL DEFAULT '',
    ADD COLUMN last_name VARCHAR(255) NOT NULL DEFAULT '';
