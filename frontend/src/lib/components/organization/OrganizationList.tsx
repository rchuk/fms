"use client";

import PaginatedList from "@/lib/components/common/PaginatedList";
import {OrganizationResponse} from "../../../../generated";
import {useContext} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import FloatingAddButton from "../common/FloatingAddButton";
import OrganizationListCard from "./OrganizationListCard";

type OrganizationListProps = {

}

export default function OrganizationList(props: OrganizationListProps) {
  const { organizationService } = useContext(ServicesContext);

  function renderCard(data: OrganizationResponse) {
    return <OrganizationListCard item={data} />
  }

  async function fetch(offset: number, limit: number): Promise<[number, OrganizationResponse[]]> {
    const response = await organizationService.listUserOrganizations({ offset, limit });

    return [response.totalCount, response.items];
  }

  return (
    <>
      <PaginatedList fetch={fetch} pageSize={10} renderItem={renderCard} />
      <FloatingAddButton />
    </>
  );
}
