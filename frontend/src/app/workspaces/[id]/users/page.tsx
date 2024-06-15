"use client";

import {WorkspaceResponse} from "../../../../../generated";
import {useContext, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import EntityPage from "@/lib/components/common/EntityPage";
import UserMemberList from "@/lib/components/user/UserMemberList";

export default function WorkspaceUsersPage({ params }: { params: { id: number } }) {
  const [workspace, setWorkspace] = useState<WorkspaceResponse | null>(null);
  const { workspaceService } = useContext(ServicesContext);

  async function fetch() {
    return await workspaceService.getWorkspace({ id: params.id });
  }

  // TODO: Also join organization role on the backend
  const canAddUser = workspace?.kind !== "PRIVATE" && (workspace?.role == "OWNER" || workspace?.role === "ADMIN");
  const addSource = workspace?.owner.organization !== undefined
    ? { kind: "organization", organizationId: workspace.owner.organization.id } as const
    : { kind: "global" } as const;

  return (
    <EntityPage id={params.id} entity={workspace} setEntity={setWorkspace} fetch={fetch}>
      <UserMemberList
        source={{ kind: "workspace", workspaceId: params.id }}
        addSource={addSource}
        enableCreation={canAddUser}
      />
    </EntityPage>
  );
}

