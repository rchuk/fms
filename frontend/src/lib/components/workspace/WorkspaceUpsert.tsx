import UpsertComponent from "@/lib/components/common/UpsertComponent";
import Grid from "@mui/material/Unstable_Grid2";
import {WorkspaceUpsertRequest} from "../../../../generated";
import {useContext, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import {TextField} from "@mui/material";
import {WorkspaceSource} from "@/lib/components/workspace/Common";


type WorkspaceUpsertProps = {
  initialId: number | null,
  source: WorkspaceSource,
  isLocked: boolean

  cancel?: () => void,
  onError?: () => void,
  onDelete?: () => void,
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
    switch (props.source.kind) {
      case "user":
        return await workspaceService.createSharedUserWorkspace({ workspaceUpsertRequest: view });
      case "organization":
        return await workspaceService.createOrganizationWorkspace({
          organizationId: props.source.organizationId,
          workspaceUpsertRequest: view
        });
    }
  }

  async function handleDelete(id: number) {
    await workspaceService.deleteWorkspace({ id });
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
      delete={!props.isLocked ? handleDelete : undefined}
      validate={validate}
      cancel={props.cancel}
      onError={props.onError}
      onSave={props.onSave}
      onDelete={props.onDelete}
      createHeader="Створення робочого простору"
      updateHeader="Редагування робочого простору"
    >
      <Grid xs={12}>
        <TextField
          label="Назва"
          required
          disabled={props.isLocked}
          fullWidth
          value={view.name}
          onChange={e => setView({...view, name: e.target.value})}
        />
      </Grid>
    </UpsertComponent>
  );
}
