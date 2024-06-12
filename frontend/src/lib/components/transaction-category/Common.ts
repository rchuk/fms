
export interface TransactionCategorySourceUser {
  kind: "user"
}

export interface TransactionCategorySourceOrganization {
  kind: "organization",
  organizationId: number
}

export interface TransactionCategorySourceWorkspace {
  kind: "workspace",
  workspaceId: number
}

export type TransactionCategorySource = TransactionCategorySourceUser | TransactionCategorySourceOrganization | TransactionCategorySourceWorkspace;
