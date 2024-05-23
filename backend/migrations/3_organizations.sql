CREATE TABLE organizations(
    id      SERIAL PRIMARY KEY,
    name    VARCHAR(255) NOT NULL
);

CREATE TABLE organization_roles(
    id      SERIAL PRIMARY KEY,
    name    VARCHAR(127) UNIQUE NOT NULL
);

INSERT INTO organization_roles(name) VALUES ('OWNER'), ('ADMIN'), ('MEMBER');

CREATE TABLE user_organization(
    user_id         INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    organization_id INTEGER NOT NULL REFERENCES organizations(id) ON DELETE CASCADE,
    role            INTEGER NOT NULL REFERENCES organization_roles(id),
    PRIMARY KEY(user_id, organization_id)
);
