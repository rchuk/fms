CREATE TABLE workspace_kinds(
    id      SERIAL PRIMARY KEY,
    name    VARCHAR(127) NOT NULL
);

INSERT INTO workspace_kinds(name) VALUES ('PERSONAL'), ('SHARED');

CREATE TABLE workspaces(
    id      SERIAL PRIMARY KEY,
    name    VARCHAR(255) NOT NULL,
    kind    INTEGER NOT NULL REFERENCES workspace_kinds(id)
);

CREATE TABLE workspace_roles(
    id      SERIAL PRIMARY KEY,
    name    VARCHAR(127) UNIQUE NOT NULL
);

CREATE TABLE account_workspace(
    account_id      INTEGER NOT NULL REFERENCES accounts(id) ON DELETE CASCADE,
    workspace_id    INTEGER NOT NULL REFERENCES workspaces(id) ON DELETE CASCADE,
    role            INTEGER NOT NULL REFERENCES workspace_roles(id),
    PRIMARY KEY(account_id, workspace_id)
);
