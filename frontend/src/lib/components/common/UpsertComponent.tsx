import {PropsWithChildren, useContext, useEffect, useState} from "react";
import UpsertContainer from "@/lib/components/common/UpsertContainer";
import ProgressSpinner from "@/lib/components/common/ProgressSpinner";
import {AlertContext} from "@/lib/services/AlertService";
import {getRequestError} from "@/lib/utils/RequestUtils";


type UpsertComponentProps<IdT> = {
  initialId: IdT | null,
  createHeader: string,
  updateHeader: string,

  resetView: () => void,
  fetch: (id: IdT) => Promise<void>,
  create: () => Promise<IdT>,
  update: (id: IdT) => Promise<void>,

  onSave?: () => void,
  cancel?: () => void,
  onError?: (reason: any) => void,
}

export default function UpsertComponent<IdT>(props: PropsWithChildren<UpsertComponentProps<IdT>>) {
  const [id, setId] = useState<IdT | null>(null);
  const [isReady, setIsReady] = useState<boolean>(false);
  const showAlert = useContext(AlertContext);

  useEffect(() => {
    if (props.initialId == null) {
      props.resetView();
      setIsReady(true);

      return;
    }

    const fetch = async() => {
      await props.fetch(props.initialId!)
    };

    setId(props.initialId);
    fetch()
      .then(_ => setIsReady(true))
      .catch(e => {
        getRequestError(e).then(m => showAlert(m, "error"))

        props.onError?.(e);
      });
  }, [props.initialId])

  function update() {
    const update = async() => {
      await props.update(id!);
    };

    update()
      .then(_ => showAlert("Інформацію оновлено", "success"))
      .then(_ => props.onSave?.())
      .catch(e => getRequestError(e).then(m => showAlert(m, "error")));
  }

  function create() {
    const create = async() => {
      const id = await props.create();
      setId(id);

      return id;
    }

    create()
      .then(_ => showAlert("Інформацію створено", "success"))
      .then(id => props.onSave?.())
      .catch(e => getRequestError(e).then(m => showAlert(m, "error")));
  }

  function submit() {
    if (id == null)
      create()
    else
      update();
  }

  function cancel() {
    props.cancel?.();
  }

  if (!isReady)
    return <ProgressSpinner />;

  return (
    <UpsertContainer submit={submit} cancel={cancel} header={id != null ? props.updateHeader : props.createHeader}>
      {props.children}
    </UpsertContainer>
  );
}
