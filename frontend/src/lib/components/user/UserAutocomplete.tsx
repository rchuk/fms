import {useContext} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import AutocompleteComponent from "@/lib/components/common/AutocompleteComponent";
import {UserResponse} from "../../../../generated";
import {UserSource} from "@/lib/components/user/Common";


type UserAutocompleteProps = {
  initialId?: number,
  source: UserSource,

  required?: boolean,
  disabled?: boolean,

  setSelectedId: (value: number | null) => void
};

export default function UserAutocomplete(props: UserAutocompleteProps) {
  const { userService, organizationService, workspaceService } = useContext(ServicesContext);

  async function fetch(query: string, count: number) {
    switch (props.source.kind) {
      case "global":
        return await userService.listUsers({ query, limit: count });
      case "organization":
        const organizationResponse = await organizationService.listOrganizationUsers({
          organizationId: props.source.organizationId,
          query,
          limit: count
        });

        return { totalCount: organizationResponse.totalCount, items: organizationResponse.items.map(item => item.user) };
      case "workspace":
        const workspaceResponse =  await workspaceService.listWorkspaceUsers({
          workspaceId: props.source.workspaceId,
          query,
          limit: count
        });

        return { totalCount: workspaceResponse.totalCount, items: workspaceResponse.items.map(item => item.user) };
    }
  }

  return (
    <AutocompleteComponent
      disabled={props.disabled}
      initialId={props.initialId}
      required={props.required}
      setSelectedId={props.setSelectedId}
      fetch={fetch}
      label={"Користувач"}
      getItemLabel={(item: UserResponse) => `${item.firstName} ${item.lastName}`}
    />
  );
}
