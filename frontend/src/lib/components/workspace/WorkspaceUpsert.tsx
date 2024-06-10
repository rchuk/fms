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

function getDefaultWorkspaceView(): WorkspaceUpsertRequest {
  return {
    name: ""
  };
}

export default function WorkspaceUpsert(props: WorkspaceUpsertProps) {
  const { workspaceService } = useContext(ServicesContext);
  const [view, setView] = useState<WorkspaceUpsertRequest>(getDefaultWorkspaceView);

  async function fetch() {
    const { id, ...newView } = await workspaceService.getWorkspace({ id: props.initialId! });
    setView(newView);
  }

  async function update(id: number) {
    await workspaceService.updateWorkspace({ id, workspaceUpsertRequest: view });
  }

  async function create() {
    return await workspaceService.createSharedUserWorkspace({ workspaceUpsertRequest: view });
  }

  return (
    <UpsertComponent
      initialId={props.initialId}
      resetView={() => setView(getDefaultWorkspaceView)}
      fetch={fetch}
      create={create}
      update={update}
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
