"use client";

import {WorkspaceResponse} from "../../../../../generated";
import {useContext, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import EntityPage from "@/lib/components/common/EntityPage";
import UserMemberList from "@/lib/components/user/UserMemberList";
import ProgressSpinner from "@/lib/components/common/ProgressSpinner";

export default function WorkspaceUsersPage({ params }: { params: { id: number } }) {
  const [workspace, setWorkspace] = useState<WorkspaceResponse | null>(null);
  const { workspaceService } = useContext(ServicesContext);

  async function fetch() {
    return await workspaceService.getWorkspace({ id: params.id });
  }

  // TODO: Add join with organization role on the backend. Currently has false positives
  const canAddUser = workspace!= null && workspace.kind !== "PRIVATE"
    && (workspace.role !== "COLLABORATOR" && workspace.role !== "VIEWER");
  const addSource = workspace != null && workspace?.owner.organization !== undefined
    ? { kind: "organization", organizationId: workspace.owner.organization.id } as const
    : { kind: "global" } as const;

  return (
    <EntityPage id={params.id} entity={workspace} setEntity={setWorkspace} fetch={fetch}>
      {
        workspace != null
          ? <UserMemberList
            source={{ kind: "workspace", workspaceId: params.id }}
            addSource={addSource}
            enableCreation={canAddUser}
          />
          : <ProgressSpinner />
      }
    </EntityPage>
  );
}

