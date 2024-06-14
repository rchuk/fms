"use client";

import {OrganizationResponse} from "../../../../../generated";
import {useContext, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import EntityPage from "@/lib/components/common/EntityPage";
import TransactionCategoryList from "@/lib/components/transaction-category/TransactionCategoryList";

export default function OrganizationPage({ params }: { params: { id: number } }) {
  const [organization, setOrganization] = useState<OrganizationResponse | null>(null);
  const { organizationService } = useContext(ServicesContext);

  async function fetch() {
    return await organizationService.getOrganization({ id: params.id });
  }

  const canCreateCategory = organization?.role !== undefined && organization?.role !== "MEMBER";

  return (
    <EntityPage id={params.id} entity={organization} setEntity={setOrganization} fetch={fetch}>
      <TransactionCategoryList source={{ kind: "organization", organizationId: params.id }} enableCreation={canCreateCategory}/>
    </EntityPage>
  );
}


