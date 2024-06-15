"use client";

import {OrganizationResponse} from "../../../../generated";
import {useContext, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import EntityPage from "@/lib/components/common/EntityPage";
import WorkspaceList from "@/lib/components/workspace/WorkspaceList";
import ProgressSpinner from "@/lib/components/common/ProgressSpinner";

export default function OrganizationPage({ params }: { params: { id: number } }) {
  const [organization, setOrganization] = useState<OrganizationResponse | null>(null);
  const { organizationService } = useContext(ServicesContext);

  async function fetch() {
    return await organizationService.getOrganization({ id: params.id });
  }

  if (organization === null)
    return <ProgressSpinner />;

  const canCreateWorkspace = organization.role === "OWNER" || organization.role === "ADMIN";

  return (
    <EntityPage id={params.id} entity={organization} setEntity={setOrganization} fetch={fetch}>
      <WorkspaceList
        source={{ kind: "organization", organizationId: params.id }}
        enableCreation={canCreateWorkspace}
      />
    </EntityPage>
  );
}

