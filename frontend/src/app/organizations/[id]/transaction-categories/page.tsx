"use client";

import {OrganizationResponse} from "../../../../../generated";
import {useContext, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import EntityPage from "@/lib/components/common/EntityPage";
import TransactionCategoryList from "@/lib/components/transaction-category/TransactionCategoryList";
import ProgressSpinner from "@/lib/components/common/ProgressSpinner";

export default function OrganizationTransactionCategoriesPage({ params }: { params: { id: number } }) {
  const [organization, setOrganization] = useState<OrganizationResponse | null>(null);
  const { organizationService } = useContext(ServicesContext);

  async function fetch() {
    return await organizationService.getOrganization({ id: params.id });
  }

  const canCreateCategory = organization != null && (organization.role === "OWNER" || organization.role === "ADMIN");

  return (
    <EntityPage id={params.id} entity={organization} setEntity={setOrganization} fetch={fetch}>
      {
        organization != null
          ? <TransactionCategoryList source={{ kind: "organization", organizationId: params.id }} enableCreation={canCreateCategory}/>
          : <ProgressSpinner />
      }
    </EntityPage>
  );
}


