CREATE TABLE transaction_category_kinds(
    id      SERIAL PRIMARY KEY,
    name    VARCHAR(127) NOT NULL
);

INSERT INTO transaction_category_kinds(name) VALUES ('INCOME'), ('EXPENSE'), ('MIXED');

CREATE TABLE transaction_categories(
    id              SERIAL PRIMARY KEY,
    workspace_id    INTEGER NOT NULL REFERENCES workspaces(id) ON DELETE CASCADE,
    name            VARCHAR(255) NOT NULL,
    kind            INTEGER NOT NULL REFERENCES transaction_category_kinds(id),
    ui_color        CHAR(6) NOT NULL
);

CREATE TABLE transactions(
    id                  SERIAL PRIMARY KEY,
    workspace_id        INTEGER NOT NULL REFERENCES workspaces(id) ON DELETE CASCADE,
    category_id         INTEGER NOT NULL REFERENCES transaction_categories(id),
    user_id             INTEGER NULL REFERENCES users(id) ON DELETE SET NULL,
    amount              BIGINT NOT NULL,
    timestamp           TIMESTAMP NOT NULL DEFAULT NOW(),
    creation_timestamp  TIMESTAMP NOT NULL DEFAULT NOW(),
    creation_user_id    INTEGER NULL REFERENCES users(id) ON DELETE SET NULL
);
