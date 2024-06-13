
export interface UserSourceGlobal {
  kind: "global"
}

export interface UserSourceWorkspace {
  kind: "workspace",
  workspaceId: number
}

export interface UserSourceOrganization {
  kind: "organization",
  organizationId: number
}

export type UserSource = UserSourceGlobal | UserSourceWorkspace | UserSourceOrganization;
