import Grid from "@mui/material/Unstable_Grid2";
import {
  OrganizationRole,
  UserResponse
} from "../../../../generated";
import {useContext, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import {UserSourceOrganization} from "@/lib/components/user/Common";
import {FormControl, InputLabel, MenuItem, Select, TextField} from "@mui/material";
import UpdateComponent from "@/lib/components/common/UpdateComponent";
import {OrganizationRole_i18} from "@/lib/i18/OrganizationRole_i18";

type OrganizationUserUpdateProps = {
  source:  UserSourceOrganization,
  user: UserResponse,
  isLocked: boolean,

  cancel?: () => void,
  onError?: () => void,
  onSave?: () => void
};

export default function OrganizationUserUpdate(props: OrganizationUserUpdateProps) {
  const { organizationService } = useContext(ServicesContext);
  const [view, setView] = useState<Partial<{ role: OrganizationRole }>>({});

  async function fetch(id: number) {
    return {
      role: await organizationService.organizationGetUserRole({
        userId: id,
        organizationId: props.source.organizationId
      })
    };
  }

  async function update(id: number, view: { role: OrganizationRole }) {
    await organizationService.organizationUpdateUserRole({
      userId: id,
      organizationId: props.source.organizationId,
      body: view.role
    });
  }

  async function handleDelete(id: number) {
    await organizationService.organizationRemoveUser({
      userId: id,
      organizationId: props.source.organizationId
    });
  }

  function validate(view: Partial<{ role: OrganizationRole }>): { role: OrganizationRole } | null {
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
            onChange={e => setView({ role: e.target.value as OrganizationRole })}
          >
            {Object.values(OrganizationRole).map(value => (
              <MenuItem key={value} value={value}>{OrganizationRole_i18[value]}</MenuItem>
            ))}
          </Select>
        </FormControl>
      </Grid>
    </UpdateComponent>
  );
}
