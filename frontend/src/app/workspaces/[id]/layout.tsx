"use client";

import PageTabs, {NavigationTabKind} from "@/lib/components/common/PageTabs";
import {useRouter} from "next/navigation";
import {useContext, useEffect, useState} from "react";
import {WorkspaceResponse} from "../../../../generated";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import {getRequestError} from "@/lib/utils/RequestUtils";
import {AlertContext} from "@/lib/services/AlertService";
import ProgressSpinner from "@/lib/components/common/ProgressSpinner";

export default function WorkspaceLayout({ children, params }: {
  children: React.ReactNode,
  params: { id: number }
}) {
  const [workspace, setWorkspace] = useState<WorkspaceResponse | null>(null);
  const { workspaceService } = useContext(ServicesContext);
  const showAlert = useContext(AlertContext);
  const router = useRouter();

  useEffect(() => {
    fetch();
  }, [params.id]);

  function fetch() {
    const fetch = async() => {
      const newOrganization = await workspaceService.getWorkspace({ id: params.id });
      setWorkspace(newOrganization);
    };

    fetch().catch(e => getRequestError(e).then(m => showAlert(m, "error")));
  }

  function navigate(tab: NavigationTabKind) {
    switch (tab) {
      case "Main":
        router.replace(`/workspaces/${params.id}`);
        return;
      case "TransactionCategories":
        router.replace(`/workspaces/${params.id}/transaction-categories`);
        return;
      case "Users":
        router.replace(`/workspaces/${params.id}/users`);
        return;
    }
  }

  if (workspace === null)
    return <ProgressSpinner />;

  const canEdit = workspace.role === "OWNER" || workspace.role === "ADMIN";

  return (
    <>
      <PageTabs navigate={navigate} mainLabel="Транзакції" header={workspace.name} canEdit={canEdit} />
      {children}
    </>
  )
}
