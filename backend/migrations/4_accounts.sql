CREATE TABLE accounts(
    id              SERIAL PRIMARY KEY,
    user_id         INTEGER NULL REFERENCES users(id) ON DELETE CASCADE,
    organization_id INTEGER NULL REFERENCES organizations(id) ON DELETE CASCADE,
    CONSTRAINT one_of CHECK ((user_id IS NULL) <> (organization_id IS NULL))
);
