"use client";

import PaginatedList from "@/lib/components/common/PaginatedList";
import {WorkspaceResponse} from "../../../../generated";
import WorkspaceListCard from "@/lib/components/workspace/WorkspaceListCard";
import {useContext} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";


export type WorkspaceListProps = {

}

export default function WorkspaceList(props: WorkspaceListProps) {
  const { workspaceService } = useContext(ServicesContext);

  function renderCard(data: WorkspaceResponse) {
    return <WorkspaceListCard id={data.id} name={data.name} />
  }
  
  async function fetch(offset: number, limit: number): Promise<[number, WorkspaceResponse[]]> {
    // TODO: Make it generic over list endpoint
    const response = await workspaceService.listUserWorkspaces({ offset, limit });

    return [response.totalCount, response.items];
  }

  return (
    <PaginatedList fetch={fetch} pageSize={10} renderItem={renderCard}/>
  );
}
