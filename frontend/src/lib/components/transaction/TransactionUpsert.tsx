import UpsertComponent from "@/lib/components/common/UpsertComponent";
import Grid from "@mui/material/Unstable_Grid2";
import {TransactionCategoryKind, TransactionCategoryResponse, TransactionUpsertRequest} from "../../../../generated";
import {useContext, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import {Checkbox, Divider, FormControlLabel, TextField} from "@mui/material";
import {DateTimePicker} from "@mui/x-date-pickers";
import dayjs from "dayjs";
import TransactionCategoryAutocomplete from "@/lib/components/transaction-category/TransactionCategoryAutocomplete";
import UserAutocomplete from "@/lib/components/user/UserAutocomplete";

type TransactionUpsertProps = {
  initialId: number | null,
  workspaceId: number,
  isLocked: boolean,

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
  const [selectedCategory, setSelectedCategory] = useState<TransactionCategoryResponse | null>(null);
  const [isMixedSpending, setIsMixedSpending] = useState<boolean>(false);

  async function fetch(id: number) {
    const { id: _, ...newView } = await transactionService.getTransaction({ id });
    const { category, ...other } = newView;

    return { categoryId: category.id, ...other };
  }

  function setAmount(newAmount: number) {
    let sign = 1;
    if (selectedCategory?.kind === TransactionCategoryKind.Income)
      sign = 1;
    else if (selectedCategory?.kind === TransactionCategoryKind.Expense)
      sign = -1;
    else if (selectedCategory?.kind === TransactionCategoryKind.Mixed)
      sign = isMixedSpending ? -1 : 1;

    setView({...view, amount: sign * newAmount});
  }

  async function update(id: number, view: TransactionUpsertRequest) {
    await transactionService.updateTransaction({ id, transactionUpsertRequest: view });
  }

  async function create(view: TransactionUpsertRequest) {
    return await transactionService.createTransaction({ workspaceId: props.workspaceId, transactionUpsertRequest: view });
  }

  async function handleDelete(id: number) {
    await transactionService.deleteTransaction({ id });
  }
  
  function validate(view: Partial<TransactionUpsertRequest>): TransactionUpsertRequest | null {
    const { categoryId, amount, ...other } = view;
    if (categoryId == null || amount == null)
      return null;

    return { categoryId, amount, ...other };
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
      createHeader="Створення транзакції"
      updateHeader={props.isLocked ? "Перегляд транзакції" : "Редагування транзакції"}
    >
      <Grid xs={6}>
        <TransactionCategoryAutocomplete
          required
          disabled={props.isLocked}
          workspaceId={props.workspaceId}
          includeOwner={true}
          initialId={view.categoryId}
          selectedCategory={selectedCategory}
          setSelectedCategory={setSelectedCategory}
          setSelectedId={v => setView({...view, categoryId: v ?? undefined})}
        />
      </Grid>
      <Grid xs={6}>
        <TextField
          label="Вартість"
          type="number"
          required
          disabled={props.isLocked}
          fullWidth
          inputProps={{ min: 1 }}
          value={view.amount != null ? Math.abs(view.amount) : ""}
          onChange={e => setAmount(Number(e.target.value))}
        />
      </Grid>
      {
        selectedCategory?.kind === TransactionCategoryKind.Mixed
          ?
          <>
            <Grid xs={6}>

            </Grid>
            <Grid xs={6}>
              <FormControlLabel
                disabled={props.isLocked}
                control={<Checkbox checked={isMixedSpending} onChange={e => setIsMixedSpending(e.target.checked)} />}
                label="Витрата"
              />
            </Grid>
          </>
          : null
      }
      <Grid xs={12}>
        <Divider />
      </Grid>
      <Grid xs={6}>
        <DateTimePicker
          label="Дата"
          disabled={props.isLocked}
          disableFuture
          slotProps={{
            actionBar: { actions: ["clear"], hidden: false },
            textField: { fullWidth: true }
          }}
          value={view.timestamp != null ? dayjs(view.timestamp) : undefined}
          onChange={value => setView({...view, timestamp: value?.toDate() ?? undefined})}
        />
      </Grid>
      <Grid xs={6}>
        <UserAutocomplete
          disabled={props.isLocked}
          initialId={view.userId}
          source={{ kind: "workspace", workspaceId: props.workspaceId }}
          setSelectedId={v => setView({...view, userId: v ?? undefined })}
        />
      </Grid>
      <Grid xs={12}>
        <TextField
          label="Опис"
          disabled={props.isLocked}
          multiline
          fullWidth
          value={view.description}
          onChange={e => setView({...view, description: e.target.value})}
        />
      </Grid>
    </UpsertComponent>
  );
}
