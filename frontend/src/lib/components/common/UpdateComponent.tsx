import {PropsWithChildren, useContext, useEffect, useState} from "react";
import {AlertContext} from "@/lib/services/AlertService";
import {ConfirmationDialogContext} from "@/lib/services/ConfirmationDialogService";
import {getRequestError} from "@/lib/utils/RequestUtils";
import ProgressSpinner from "@/lib/components/common/ProgressSpinner";
import UpsertContainer from "@/lib/components/common/UpsertContainer";

type UpdateComponentProps<IdT, ViewT> = {
  id: IdT,
  header: string,

  view: Partial<ViewT>,
  setView: (value: Partial<ViewT>) => void,

  fetch: (id: IdT) => Promise<ViewT>,
  update: (id: IdT, view: ViewT) => Promise<void>,
  delete?: (id: IdT) => Promise<void>,
  validate: (view: Partial<ViewT>) => ViewT | null,

  onSave?: () => void,
  cancel?: () => void,
  onError?: (reason: any) => void,
}

export default function UpdateComponent<IdT, ViewT>(props: PropsWithChildren<UpdateComponentProps<IdT, ViewT>>) {
  const [isReady, setIsReady] = useState<boolean>(false);
  const showAlert = useContext(AlertContext);
  const showConfirmation = useContext(ConfirmationDialogContext);

  useEffect(() => {
    const fetch = async() => {
      const view = await props.fetch(props.id);

      props.setView(view);
      setIsReady(true);
    };

    fetch()
      .catch(e => {
        getRequestError(e).then(m => showAlert(m, "error"))

        props.onError?.(e);
      });
  }, [props.id])

  function update() {
    const view = props.validate(props.view);
    if (view === null)
      return;

    const update = async() => {
      await props.update(props.id, view);
    };

    update()
      .then(_ => showAlert("Інформацію оновлено", "success"))
      .then(_ => props.onSave?.())
      .catch(e => getRequestError(e).then(m => showAlert(m, "error")));
  }

  function handleDelete() {
    if (props.id === null)
      return;

    const callback = () => {
      props.delete?.(props.id)
        .then(_ => showAlert("Інформацію видалено", "success"))
        .then(_ => props.onSave?.())
        .catch(e => getRequestError(e).then(m => showAlert(m, "error")))
    };

    showConfirmation({
      confirm: callback
    });
  }
  function cancel() {
    props.cancel?.();
  }

  if (!isReady)
    return <ProgressSpinner />;

  return (
    <UpsertContainer
      submit={update}
      cancel={cancel}
      header={props.header}
      delete={props.delete != null ? handleDelete : undefined}
    >
      {props.children}
    </UpsertContainer>
  );
}
