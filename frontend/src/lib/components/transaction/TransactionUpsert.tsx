import UpsertComponent from "@/lib/components/common/UpsertComponent";
import Grid from "@mui/material/Unstable_Grid2";
import {TransactionUpsertRequest} from "../../../../generated";
import {useContext, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import {Divider, TextField} from "@mui/material";
import {DateTimePicker} from "@mui/x-date-pickers";
import dayjs from "dayjs";
import TransactionCategoryAutocomplete from "@/lib/components/transaction-category/TransactionCategoryAutocomplete";

type TransactionUpsertProps = {
  initialId: number | null,
  workspaceId: number,

  cancel?: () => void,
  onError?: () => void,
  onSave?: () => void
};

function getDefaultTransactionView(): Partial<TransactionUpsertRequest> {
  return {};
}

export default function TransactionUpsert(props: TransactionUpsertProps) {
  const { transactionService } = useContext(ServicesContext);
  const [view, setView] = useState<Partial<TransactionUpsertRequest>>(getDefaultTransactionView);

  async function fetch(id: number) {
    const { id: _, ...newView } = await transactionService.getTransaction({ id });
    const { category, ...other } = newView;

    return { categoryId: category.id, ...other };
  }

  async function update(id: number, view: TransactionUpsertRequest) {
    await transactionService.updateTransaction({ id, transactionUpsertRequest: view });
  }

  async function create(view: TransactionUpsertRequest) {
    return await transactionService.createTransaction({ workspaceId: props.workspaceId, transactionUpsertRequest: view });
  }
  
  function validate(view: Partial<TransactionUpsertRequest>): TransactionUpsertRequest | null {
    const { categoryId, amount, ...other } = view;
    if (categoryId == null || amount == null)
      return null;

    return { categoryId, amount, ...other };
  }

  // TODO: Handle so category type and amount sign match

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
      createHeader="Створення транзакції"
      updateHeader="Редагування транзакції"
    >
      <Grid xs={6}>
        <TransactionCategoryAutocomplete
          required
          workspaceId={props.workspaceId}
          setSelectedId={v => setView({...view, categoryId: v ?? undefined})}
        />
      </Grid>
      <Grid xs={6}>
        <TextField
          label="Вартість"
          type="number"
          required
          fullWidth
          value={view.amount}
          onChange={e => setView({...view, amount: Number(e.target.value)})}
        />
      </Grid>
      <Grid xs={12}>
        <Divider />
      </Grid>
      <Grid xs={6}>
        <DateTimePicker
          label="Дата"
          disableFuture
          slotProps={{
            actionBar: { actions: ["clear"], hidden: false },
            textField: { fullWidth: true }
          }}
          value={view.timestamp != null ? dayjs(view.timestamp) : undefined}
          onChange={value => setView({...view, timestamp: (value ?? dayjs())?.toDate()})}
        />
      </Grid>

      <Grid xs={12}>
        <TextField
          label="Опис"
          multiline
          fullWidth
          value={view.description}
          onChange={e => setView({...view, description: e.target.value})}
        />
      </Grid>
    </UpsertComponent>
  );
}
