"use client";

import PageTabs, {NavigationTabKind} from "@/lib/components/common/PageTabs";
import {useRouter} from "next/navigation";
import {ReactElement, useContext, useEffect, useState} from "react";
import {OrganizationResponse} from "../../../../generated";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import {AlertContext} from "@/lib/services/AlertService";
import {getRequestError} from "@/lib/utils/RequestUtils";
import ProgressSpinner from "@/lib/components/common/ProgressSpinner";
import OrganizationUpsert from "@/lib/components/organization/OrganizationUpsert";
import ModalComponent, {useModalClosingCallback, useModalControls} from "@/lib/components/common/ModalComponent";

export default function OrganizationLayout({ children, params }: {
  children: React.ReactNode,
  params: { id: number }
}) {
  const [organization, setOrganization] = useState<OrganizationResponse | null>(null);
  const [modalContent, setModalContent] = useState<ReactElement | null>(null);
  const [isDirty, setIsDirty] = useState<boolean>(false);
  const { organizationService } = useContext(ServicesContext);
  const showAlert = useContext(AlertContext);

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
      const newOrganization = await organizationService.getOrganization({ id: params.id });
      setOrganization(newOrganization);
    };

    fetch().catch(e => getRequestError(e).then(m => showAlert(m, "error")));
  }

  const router = useRouter();

  function navigate(tab: NavigationTabKind) {
    switch (tab) {
      case "Main":
        router.replace(`/organizations/${params.id}`);
        return;
      case "TransactionCategories":
        router.replace(`/organizations/${params.id}/transaction-categories`);
        return;
      case "Users":
        router.replace(`/organizations/${params.id}/users`);
        return;
    }
  }

  if (organization === null)
    return <ProgressSpinner />;

  const canEdit = organization.role === "OWNER" || organization.role === "ADMIN";
  const canDelete = organization.role === "OWNER";

  function edit() {
    return openModal(<OrganizationUpsert
      isLocked={!canEdit}
      initialId={params.id}
      cancel={closeModal}
      onSave={onSave}
      onDelete={canDelete ? onDelete : undefined}
    />);
  }

  return (
    <>
      <PageTabs navigate={navigate} mainLabel="Робочі простори" header={organization.name} canEdit={canEdit} onEdit={edit} />
      {children}
      <ModalComponent content={modalContent} setContent={setModalContent} />
    </>
  )
}
