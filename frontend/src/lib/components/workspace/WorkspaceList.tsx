"use client";

import PaginatedList from "@/lib/components/common/PaginatedList";
import {WorkspaceResponse} from "../../../../generated";
import WorkspaceListCard from "@/lib/components/workspace/WorkspaceListCard";
import {ReactElement, useContext, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import FloatingAddButton from "../common/FloatingAddButton";
import {Dialog, DialogContent} from "@mui/material";
import WorkspaceUpsert from "@/lib/components/workspace/WorkspaceUpsert";

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
  const [modalContent, setModalContent] = useState<ReactElement | null>(null);
  const [isDirty, setIsDirty] = useState<boolean>(false);
  const { workspaceService } = useContext(ServicesContext);

  function closeModal() {
    setModalContent(null);
  }

  function openModal(content: ReactElement) {
    setModalContent(content);
  }

  function onSave(callback?: () => void): () => void {
    return () => {
      callback?.();
      setIsDirty(true);
      closeModal();
    };
  }

  function create(callback?: () => void) {
    openModal(<WorkspaceUpsert initialId={null} onError={closeModal} cancel={closeModal} onSave={onSave(callback)} />);
  }

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
      <PaginatedList fetch={fetch} pageSize={10} renderItem={renderCard} isDirty={isDirty} setIsDirty={setIsDirty}/>
      <FloatingAddButton onClick={() => create()}/>
      <Dialog open={modalContent !== null} onClose={closeModal}>
        <DialogContent sx={{ padding: 4}}>
          {modalContent}
        </DialogContent>
      </Dialog>
    </>
  );
}
