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

function getDefaultOrganizationView(): Partial<OrganizationUpsertRequest> {
  return {};
}

export default function OrganizationUpsert(props: OrganizationUpsertProps) {
  const { organizationService } = useContext(ServicesContext);
  const [view, setView] = useState<Partial<OrganizationUpsertRequest>>(getDefaultOrganizationView);

  async function fetch(id: number) {
    const { id: _, ...newView } = await organizationService.getOrganization({ id });

    return newView;
  }

  async function update(id: number, view: OrganizationUpsertRequest) {
    await organizationService.updateOrganization({ id, organizationUpsertRequest: view });
  }

  async function create(view: OrganizationUpsertRequest) {
    return await organizationService.createOrganization({ organizationUpsertRequest: view });
  }

  function validate(view: Partial<OrganizationUpsertRequest>): OrganizationUpsertRequest | null {
    const { name, ...other } = view;
    if (name == null)
      return null;

    return { name, ...other };
  }

  return (
    <UpsertComponent
      view={view}
      setView={setView}
      initialId={props.initialId}
      fetch={fetch}
      create={create}
      update={update}
      validate={validate}
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
