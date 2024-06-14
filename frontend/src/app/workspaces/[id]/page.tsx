"use client";

import TransactionList from "@/lib/components/transaction/TransactionList";
import {WorkspaceResponse} from "../../../../generated";
import {useContext, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import EntityPage from "@/lib/components/common/EntityPage";

export default function WorkspacePage({ params }: { params: { id: number } }) {
  const [workspace, setWorkspace] = useState<WorkspaceResponse | null>(null);
  const { workspaceService } = useContext(ServicesContext);

  async function fetch() {
    return await workspaceService.getWorkspace({ id: params.id });
  }

  const canCreateTransaction = workspace?.role !== undefined && workspace?.role !== "VIEWER";

  return (
    <EntityPage id={params.id} entity={workspace} setEntity={setWorkspace} fetch={fetch}>
      <TransactionList workspaceId={params.id} enableCreation={canCreateTransaction} />
    </EntityPage>
  );
}
