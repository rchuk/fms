"use client";

import TransactionList from "@/lib/components/transaction/TransactionList";
import {WorkspaceResponse} from "../../../../generated";
import {useContext, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import EntityPage from "@/lib/components/common/EntityPage";
import ProgressSpinner from "@/lib/components/common/ProgressSpinner";

export default function WorkspacePage({ params }: { params: { id: number } }) {
  const [workspace, setWorkspace] = useState<WorkspaceResponse | null>(null);
  const { workspaceService } = useContext(ServicesContext);

  async function fetch() {
    return await workspaceService.getWorkspace({ id: params.id });
  }

  const canCreateTransaction = workspace != null && workspace.role !== "VIEWER";

  return (
    <EntityPage id={params.id} entity={workspace} setEntity={setWorkspace} fetch={fetch}>
      {
        workspace != null
          ? <TransactionList workspaceId={params.id} enableCreation={canCreateTransaction} />
          : <ProgressSpinner />
      }
    </EntityPage>
  );
}
