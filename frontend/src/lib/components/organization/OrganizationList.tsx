"use client";

import PaginatedList from "@/lib/components/common/PaginatedList";
import {OrganizationResponse} from "../../../../generated";
import {ReactElement, useContext, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import FloatingAddButton from "../common/FloatingAddButton";
import OrganizationListCard from "./OrganizationListCard";
import ModalComponent, {useModalClosingCallback, useModalControls} from "@/lib/components/common/ModalComponent";
import OrganizationUpsert from "./OrganizationUpsert";

type OrganizationListProps = {

}

export default function OrganizationList(props: OrganizationListProps) {
  const [modalContent, setModalContent] = useState<ReactElement | null>(null);
  const [isDirty, setIsDirty] = useState<boolean>(false);
  const { organizationService } = useContext(ServicesContext);

  const [openModal, closeModal] = useModalControls(setModalContent);
  const onSave = useModalClosingCallback(setModalContent, () => setIsDirty(true));

  function create() {
    openModal(<OrganizationUpsert initialId={null} onError={closeModal} cancel={closeModal} onSave={onSave} />);
  }

  function renderCard(data: OrganizationResponse) {
    return <OrganizationListCard item={data} />
  }

  async function fetch(offset: number, limit: number): Promise<[number, OrganizationResponse[]]> {
    const response = await organizationService.listUserOrganizations({ offset, limit });

    return [response.totalCount, response.items];
  }

  return (
    <>
      <PaginatedList fetch={fetch} pageSize={10} renderItem={renderCard} isDirty={isDirty} setIsDirty={setIsDirty} />
      <FloatingAddButton onClick={create} />
      <ModalComponent content={modalContent} setContent={setModalContent}/>
    </>
  );
}
