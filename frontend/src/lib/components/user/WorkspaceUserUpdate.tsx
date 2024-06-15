import Grid from "@mui/material/Unstable_Grid2";
import {
  UserResponse,
  WorkspaceRole,
} from "../../../../generated";
import {useContext, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import {UserSourceWorkspace} from "@/lib/components/user/Common";
import {FormControl, InputLabel, MenuItem, Select, TextField} from "@mui/material";
import {WorkspaceRole_i18} from "@/lib/i18/WorkspaceRole_i18";
import UpdateComponent from "@/lib/components/common/UpdateComponent";

type WorkspaceUserUpdateProps = {
  source:  UserSourceWorkspace,
  user: UserResponse,
  isLocked: boolean,

  cancel?: () => void,
  onError?: () => void,
  onSave?: () => void
};

export default function WorkspaceUserUpdateProps(props: WorkspaceUserUpdateProps) {
  const { workspaceService } = useContext(ServicesContext);
  const [view, setView] = useState<Partial<{ role: WorkspaceRole }>>({});

  async function fetch(id: number) {
    return {
      role: await workspaceService.workspaceGetUserRole({
        userId: id,
        workspaceId: props.source.workspaceId
      })
    };
  }

  async function update(id: number, view: { role: WorkspaceRole }) {
    await workspaceService.workspaceUpdateUserRole({
      userId: id,
      workspaceId: props.source.workspaceId,
      body: view.role
    });
  }

  async function handleDelete(id: number) {
    await workspaceService.workspaceRemoveUser({
      userId: id,
      workspaceId: props.source.workspaceId
    });
  }

  function validate(view: Partial<{ role: WorkspaceRole }>): { role: WorkspaceRole } | null {
    return view.role !== undefined ? { role: view.role } : null;
  }

  return (
    <UpdateComponent
      id={props.user.id}
      view={view}
      setView={setView}
      fetch={fetch}
      update={update}
      validate={validate}
      cancel={props.cancel}
      onError={props.onError}
      onSave={props.onSave}
      header={"Редагування прав користувача"}
      delete={handleDelete}
    >
      <Grid xs={6}>
        <TextField
          label="Користувач"
          disabled
          required
          fullWidth
          value={`${props.user.firstName} ${props.user.lastName}`}
        />
      </Grid>
      <Grid xs={6}>
        <FormControl fullWidth>
          <InputLabel>Роль</InputLabel>
          <Select
            label="Роль"
            disabled={props.isLocked}
            value={view.role ?? ""}
            onChange={e => setView({ role: e.target.value as WorkspaceRole })}
          >
            {Object.values(WorkspaceRole).map(value => (
              <MenuItem key={value} value={value}>{WorkspaceRole_i18[value]}</MenuItem>
            ))}
          </Select>
        </FormControl>
      </Grid>
    </UpdateComponent>
  );
}
