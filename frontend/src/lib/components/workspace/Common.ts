
export interface WorkspaceSourceUser {
  kind: "user"
}

export interface WorkspaceSourceOrganization {
  kind: "organization",
  organizationId: number
}

export type WorkspaceSource = WorkspaceSourceUser | WorkspaceSourceOrganization;
