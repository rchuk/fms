"use client";

import PaginatedList from "@/lib/components/common/PaginatedList";
import {
  OrganizationRole,
  OrganizationUserResponse, UserResponse,
  WorkspaceRole,
  WorkspaceUserResponse
} from "../../../../generated";
import {ReactElement, useContext, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import FloatingAddButton from "../common/FloatingAddButton";
import ModalComponent, {useModalClosingCallback, useModalControls} from "../common/ModalComponent";
import {UserSource, UserSourceOrganization, UserSourceWorkspace} from "@/lib/components/user/Common";
import UserMemberListCard from "@/lib/components/user/UserMemberListCard";
import {renderWorkspaceRole} from "@/lib/components/workspace/WorkspaceListCard";
import {renderOrganizationRole} from "@/lib/components/organization/OrganizationListCard";
import WorkspaceUserUpdate from "@/lib/components/user/WorkspaceUserUpdate";
import OrganizationUserUpdate from "@/lib/components/user/OrganizationUserUpdate";
import UserMemberCreate from "@/lib/components/user/UserMemberCreate";

type UserListProps = {
  source: UserSourceWorkspace | UserSourceOrganization,
  addSource: UserSource,

  enableCreation?: boolean
}

export default function UserMemberList(props: UserListProps) {
  const [modalContent, setModalContent] = useState<ReactElement | null>(null);
  const [isDirty, setIsDirty] = useState<boolean>(false);
  const { workspaceService, organizationService } = useContext(ServicesContext);

  const [openModal, closeModal] = useModalControls(setModalContent);
  const onSave = useModalClosingCallback(setModalContent, () => setIsDirty(true));

  function create() {
    return openModal(<UserMemberCreate source={props.source} addSource={props.addSource} cancel={closeModal} onSave={onSave} />);
  }

  function update(user: UserResponse) {
    switch (props.source.kind) {
      case "workspace":
        return openModal(<WorkspaceUserUpdate user={user} source={props.source} onError={closeModal} cancel={closeModal} onSave={onSave} isLocked={!props.enableCreation} />);
      case "organization":
        return openModal(<OrganizationUserUpdate user={user} source={props.source} onError={closeModal} cancel={closeModal} onSave={onSave} isLocked={!props.enableCreation} />);
    }
  }

  function renderCard(data: WorkspaceUserResponse | OrganizationUserResponse) {
    return (<UserMemberListCard
      item={data.user}
      role={data.role}
      onClick={update}
      renderRole={role => {
        switch (props.source.kind) {
          case "workspace":
            return renderWorkspaceRole(role as WorkspaceRole)();
          case "organization":
            return renderOrganizationRole(role as OrganizationRole)();
        }
      }}
    />);
  }

  async function fetch(offset: number, limit: number) {
    switch (props.source.kind) {
      case "organization":
        return await organizationService.listOrganizationUsers({
          organizationId: props.source.organizationId,
          offset,
          limit
        });
      case "workspace":
        return await workspaceService.listWorkspaceUsers({
          workspaceId: props.source.workspaceId,
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
