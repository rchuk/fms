import {PropsWithChildren, useContext, useEffect} from "react";
import {AlertContext} from "@/lib/services/AlertService";
import {getRequestError} from "@/lib/utils/RequestUtils";
import {Box} from "@mui/material";
import {BaseEntity, EntityId} from "@/lib/utils/EntityUtils";


type EntityPageProps<EntityT extends BaseEntity<IdT>, IdT extends EntityId> = {
  id: IdT,

  entity: EntityT | null,
  setEntity: (value: EntityT | null) => void,

  fetch: (id: IdT) => Promise<EntityT>
}

export default function EntityPage<EntityT extends BaseEntity<IdT>, IdT extends EntityId>(props: PropsWithChildren<EntityPageProps<EntityT, IdT>>) {
  const showAlert = useContext(AlertContext);

  function fetch() {
    const fetch = async() => {
      const response = await props.fetch(props.id);
      props.setEntity(response);
    };

    fetch().catch(e => getRequestError(e).then(m => showAlert(m, "error")))
  }

  useEffect(() => {
    fetch();
  }, [props.id]);

  return (
    <Box display="flex" flexDirection="column" height="100%" padding={2} boxSizing="border-box">
      {props.children}
    </Box>
  );
}
