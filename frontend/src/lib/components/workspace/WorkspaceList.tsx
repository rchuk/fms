"use client";

import PaginatedList from "@/lib/components/common/PaginatedList";
import {WorkspaceResponse} from "../../../../generated";
import WorkspaceListCard from "@/lib/components/workspace/WorkspaceListCard";
import {ReactElement, useContext, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import FloatingAddButton from "../common/FloatingAddButton";
import WorkspaceUpsert from "@/lib/components/workspace/WorkspaceUpsert";
import ModalComponent, {useModalClosingCallback, useModalControls} from "../common/ModalComponent";
import {WorkspaceSource} from "@/lib/components/workspace/Common";

type WorkspaceListProps = {
  source: WorkspaceSource,

  enableCreation?: boolean
}

export default function WorkspaceList(props: WorkspaceListProps) {
  const [modalContent, setModalContent] = useState<ReactElement | null>(null);
  const [isDirty, setIsDirty] = useState<boolean>(false);
  const { workspaceService } = useContext(ServicesContext);

  const [openModal, closeModal] = useModalControls(setModalContent);
  const onSave = useModalClosingCallback(setModalContent, () => setIsDirty(true));

  function create() {
    openModal(<WorkspaceUpsert initialId={null} source={props.source} onError={closeModal} cancel={closeModal} onSave={onSave} isLocked={false} />);
  }

  function renderCard(data: WorkspaceResponse) {
    return <WorkspaceListCard item={data} />
  }

  async function fetch(offset: number, limit: number) {
    switch (props.source.kind) {
      case "user":
        return await workspaceService.listUserWorkspaces({ offset, limit });
      case "organization":
        return await workspaceService.listOrganizationWorkspaces({
          organizationId: props.source.organizationId,
          offset,
          limit
        });
    }
  }

  return (
    <>
      <PaginatedList fetch={fetch} pageSize={10} renderItem={renderCard} isDirty={isDirty} setIsDirty={setIsDirty}/>
      { props.enableCreation && <FloatingAddButton onClick={create}/> }
      <ModalComponent content={modalContent} setContent={setModalContent}/>
    </>
  );
}
