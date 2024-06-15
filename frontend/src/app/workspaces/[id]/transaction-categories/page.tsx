"use client";

import {WorkspaceResponse} from "../../../../../generated";
import {useContext, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import EntityPage from "@/lib/components/common/EntityPage";
import TransactionCategoryList from "@/lib/components/transaction-category/TransactionCategoryList";
import ProgressSpinner from "@/lib/components/common/ProgressSpinner";

export default function WorkspaceTransactionCategoriesPage({ params }: { params: { id: number } }) {
  const [workspace, setWorkspace] = useState<WorkspaceResponse | null>(null);
  const { workspaceService } = useContext(ServicesContext);

  async function fetch() {
    return await workspaceService.getWorkspace({ id: params.id });
  }

  // If we are not "VIEWER" and have access to the workspace, then we have admin role within the organization
  const canCreateCategory = workspace != null && workspace.role !== "VIEWER";

  return (
    <EntityPage id={params.id} entity={workspace} setEntity={setWorkspace} fetch={fetch}>
      {
        workspace != null
          ? <TransactionCategoryList source={{ kind: "workspace", workspaceId: params.id }} enableCreation={canCreateCategory} />
          : <ProgressSpinner />
      }
    </EntityPage>
  );
}

