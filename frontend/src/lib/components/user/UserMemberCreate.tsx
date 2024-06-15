import {UserSource, UserSourceOrganization, UserSourceWorkspace} from "@/lib/components/user/Common";
import UpsertContainer from "@/lib/components/common/UpsertContainer";
import {useContext, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import {AlertContext} from "@/lib/services/AlertService";
import {getRequestError} from "@/lib/utils/RequestUtils";
import UserAutocomplete from "@/lib/components/user/UserAutocomplete";
import Grid from "@mui/material/Unstable_Grid2";


type UserMemberCreateProps = {
  onSave?: () => void,
  cancel?: () => void,
  source: UserSourceOrganization | UserSourceWorkspace,
  addSource: UserSource
}

export default function UserMemberCreate(props: UserMemberCreateProps) {
  const [userId, setUserId] = useState<number | null>(null);
  const { workspaceService, organizationService } = useContext(ServicesContext);
  const showAlert = useContext(AlertContext);

  async function create(id: number) {
    switch (props.source.kind) {
      case "workspace":
        return await workspaceService.workspaceAddUser({
          userId: id,
          workspaceId: props.source.workspaceId
        });
      case "organization":
        return await organizationService.organizationAddUser({
          userId: id,
          organizationId: props.source.organizationId
        });
    }
  }

  function handleCreate() {
    if (userId === null)
      return;

    create(userId)
      .then(_ => showAlert("Інформацію створена", "success"))
      .then(_ => props.onSave?.())
      .catch(e => getRequestError(e).then(m => showAlert(m, "error")));
  }

  return (
    <UpsertContainer
      submit={handleCreate}
      cancel={() => props.cancel?.()}
      header={"Додати користувача"}
    >
      <Grid xs={12}>
        <UserAutocomplete
          required
          source={props.addSource}
          initialId={userId ?? undefined}
          setSelectedId={setUserId}
        />
      </Grid>
    </UpsertContainer>
  );
}
