import UpsertComponent from "@/lib/components/common/UpsertComponent";
import Grid from "@mui/material/Unstable_Grid2";
import {WorkspaceUpsertRequest} from "../../../../generated";
import {useContext, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import {TextField} from "@mui/material";


type WorkspaceUpsertProps = {
  initialId: number | null,

  cancel?: () => void,
  onError?: () => void,
  onSave?: () => void
};

function getDefaultWorkspaceView(): Partial<WorkspaceUpsertRequest> {
  return {};
}

export default function WorkspaceUpsert(props: WorkspaceUpsertProps) {
  const { workspaceService } = useContext(ServicesContext);
  const [view, setView] = useState<Partial<WorkspaceUpsertRequest>>(getDefaultWorkspaceView);

  async function fetch(id: number) {
    const { id: _, ...newView } = await workspaceService.getWorkspace({ id });

    return newView;
  }

  async function update(id: number, view: WorkspaceUpsertRequest) {
    await workspaceService.updateWorkspace({ id, workspaceUpsertRequest: view });
  }

  async function create(view: WorkspaceUpsertRequest) {
    return await workspaceService.createSharedUserWorkspace({ workspaceUpsertRequest: view });
  }

  function validate(view: Partial<WorkspaceUpsertRequest>): WorkspaceUpsertRequest | null {
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
      createHeader="Створення робочого простору"
      updateHeader="Редагування робочого простору"
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
