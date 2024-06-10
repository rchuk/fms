"use client";

import PaginatedList from "@/lib/components/common/PaginatedList";
import {WorkspaceResponse} from "../../../../generated";
import WorkspaceListCard from "@/lib/components/workspace/WorkspaceListCard";
import {useContext} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import FloatingAddButton from "../common/FloatingAddButton";

export interface WorkspaceSourceUser {
  kind: "user"
}

export interface WorkspaceSourceOrganization {
  kind: "organization",
  organizationId: number
}

type WorkspaceSource = WorkspaceSourceUser | WorkspaceSourceOrganization;

type WorkspaceListProps = {
  source: WorkspaceSource
}

export default function WorkspaceList(props: WorkspaceListProps) {
  const { workspaceService } = useContext(ServicesContext);

  function renderCard(data: WorkspaceResponse) {
    return <WorkspaceListCard item={data} />
  }

  async function fetchImpl(source: WorkspaceSource, offset: number, limit: number) {
    switch (source.kind) {
      case "user":
        return await workspaceService.listUserWorkspaces({ offset, limit });
      case "organization":
        return await workspaceService.listOrganizationWorkspaces({ organizationId: source.organizationId, offset, limit });
    }
  }
  
  async function fetch(offset: number, limit: number): Promise<[number, WorkspaceResponse[]]> {
    const response = await fetchImpl(props.source, offset, limit);

    return [response.totalCount, response.items];
  }

  return (
    <>
      <PaginatedList fetch={fetch} pageSize={10} renderItem={renderCard}/>
      <FloatingAddButton />
    </>
  );
}
