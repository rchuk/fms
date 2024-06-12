import {PropsWithChildren, useContext, useEffect, useState} from "react";
import UpsertContainer from "@/lib/components/common/UpsertContainer";
import ProgressSpinner from "@/lib/components/common/ProgressSpinner";
import {AlertContext} from "@/lib/services/AlertService";
import {getRequestError} from "@/lib/utils/RequestUtils";

type UpsertComponentProps<IdT, ViewT> = {
  view: Partial<ViewT>,
  setView: (value: Partial<ViewT>) => void,
  
  initialId: IdT | null,
  createHeader: string,
  updateHeader: string,

  fetch: (id: IdT) => Promise<ViewT>,
  create: (view: ViewT) => Promise<IdT>,
  update: (id: IdT, view: ViewT) => Promise<void>,
  validate: (view: Partial<ViewT>) => ViewT | null,

  onSave?: () => void,
  cancel?: () => void,
  onError?: (reason: any) => void,
}

export default function UpsertComponent<IdT, ViewT>(props: PropsWithChildren<UpsertComponentProps<IdT, ViewT>>) {
  const [id, setId] = useState<IdT | null>(null);
  const [isReady, setIsReady] = useState<boolean>(false);
  const showAlert = useContext(AlertContext);

  useEffect(() => {
    if (props.initialId === null) {
      props.setView({});
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
    const view = props.validate(props.view);
    if (view === null)
      return;

    const update = async() => {
      await props.update(id!, view);
    };

    update()
      .then(_ => showAlert("Інформацію оновлено", "success"))
      .then(_ => props.onSave?.())
      .catch(e => getRequestError(e).then(m => showAlert(m, "error")));
  }

  function create() {
    const view = props.validate(props.view);
    if (view === null)
      return;

    const create = async() => {
      const view = props.validate(props.view);
      if (view === null)
        return;

      const id = await props.create(view);
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
