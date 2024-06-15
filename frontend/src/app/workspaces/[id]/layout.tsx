"use client";

import PageTabs, {NavigationTabKind} from "@/lib/components/common/PageTabs";
import {useRouter} from "next/navigation";
import {ReactElement, useContext, useEffect, useState} from "react";
import {WorkspaceResponse} from "../../../../generated";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import {getRequestError} from "@/lib/utils/RequestUtils";
import {AlertContext} from "@/lib/services/AlertService";
import ProgressSpinner from "@/lib/components/common/ProgressSpinner";
import ModalComponent, {useModalClosingCallback, useModalControls} from "@/lib/components/common/ModalComponent";
import WorkspaceUpsert from "@/lib/components/workspace/WorkspaceUpsert";

export default function WorkspaceLayout({ children, params }: {
  children: React.ReactNode,
  params: { id: number }
}) {
  const [workspace, setWorkspace] = useState<WorkspaceResponse | null>(null);
  const [modalContent, setModalContent] = useState<ReactElement | null>(null);
  const [isDirty, setIsDirty] = useState<boolean>(false);
  const { workspaceService } = useContext(ServicesContext);
  const showAlert = useContext(AlertContext);
  const router = useRouter();

  const [openModal, closeModal] = useModalControls(setModalContent);
  const onSave = useModalClosingCallback(setModalContent, () => setIsDirty(true));
  const onDelete = useModalClosingCallback(setModalContent, () => router.back());

  useEffect(() => {
    fetch();
  }, [params.id]);

  useEffect(() => {
    if (isDirty) {
      fetch();
      setIsDirty(false);
    }
  }, [isDirty]);

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

  // TODO: Add join with organization role on the backend. Currently has false positives
  const canEdit = workspace.role !== "COLLABORATOR" && workspace.role !== "VIEWER";
  const canDelete = workspace.role !== "ADMIN" && workspace.role !== "COLLABORATOR" && workspace.role !== "VIEWER";

  function edit() {
    return openModal(<WorkspaceUpsert
      isLocked={!canEdit}
      initialId={params.id}
      source={null!}
      cancel={closeModal}
      onSave={onSave}
      onDelete={canDelete ? onDelete : undefined}
    />);
  }

  return (
    <>
      <PageTabs navigate={navigate} mainLabel="Транзакції" header={workspace.name} canEdit={canEdit} onEdit={edit} />
      {children}
      <ModalComponent content={modalContent} setContent={setModalContent} />
    </>
  )
}
