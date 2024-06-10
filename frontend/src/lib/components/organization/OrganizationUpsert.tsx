import UpsertComponent from "@/lib/components/common/UpsertComponent";
import Grid from "@mui/material/Unstable_Grid2";
import {OrganizationUpsertRequest} from "../../../../generated";
import {useContext, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import {TextField} from "@mui/material";


type OrganizationUpsertProps = {
  initialId: number | null,

  cancel?: () => void,
  onError?: () => void,
  onSave?: () => void
};

function getDefaultOrganizationView(): OrganizationUpsertRequest {
  return {
    name: ""
  };
}

export default function OrganizationUpsert(props: OrganizationUpsertProps) {
  const { organizationService } = useContext(ServicesContext);
  const [view, setView] = useState<OrganizationUpsertRequest>(getDefaultOrganizationView);

  async function fetch() {
    const { id, ...newView } = await organizationService.getOrganization({ id: props.initialId! });
    setView(newView);
  }

  async function update(id: number) {
    await organizationService.updateOrganization({ id, organizationUpsertRequest: view });
  }

  async function create() {
    return await organizationService.createOrganization({ organizationUpsertRequest: view });
  }

  return (
    <UpsertComponent
      initialId={props.initialId}
      resetView={() => setView(getDefaultOrganizationView)}
      fetch={fetch}
      create={create}
      update={update}
      cancel={props.cancel}
      onError={props.onError}
      onSave={props.onSave}
      createHeader="Створення організації"
      updateHeader="Редагування організації"
    >
      <Grid xs={12}>
        <TextField
          label="Назва"
          required
          fullWidth
          value={view.name}
          onChange={e => setView({...view, name: e.target.value})}
        />
      </Grid>
    </UpsertComponent>
  );
}
