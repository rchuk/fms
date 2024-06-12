import UpsertComponent from "@/lib/components/common/UpsertComponent";
import Grid from "@mui/material/Unstable_Grid2";
import {TransactionCategoryKind, TransactionCategoryUpsertRequest} from "../../../../generated";
import {useContext, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import {Divider, FormControl, InputLabel, MenuItem, Select, TextField} from "@mui/material";
import {TransactionCategorySource} from "@/lib/components/transaction-category/Common";
import {TransactionCategoryKind_i18} from "@/lib/i18/TransactionCategoryKind_i18";

type TransactionCategoryUpsertProps = {
  initialId: number | null,
  source: TransactionCategorySource,

  cancel?: () => void,
  onError?: () => void,
  onSave?: () => void
};

function getDefaultTransactionCategoryView(): Partial<TransactionCategoryUpsertRequest> {
  return {};
}

export default function TransactionCategoryUpsert(props: TransactionCategoryUpsertProps) {
  const { transactionCategoryService } = useContext(ServicesContext);
  const [view, setView] = useState<Partial<TransactionCategoryUpsertRequest>>(getDefaultTransactionCategoryView);

  async function fetch(id: number) {
    const { id: _, ...newView } = await transactionCategoryService.getTransactionCategory({ id });

    return newView;
  }

  async function update(id: number, view: TransactionCategoryUpsertRequest) {
    await transactionCategoryService.updateTransactionCategory({ id, transactionCategoryUpsertRequest: view });
  }

  async function create(view: TransactionCategoryUpsertRequest) {
    switch (props.source.kind) {
      case "user":
        return await transactionCategoryService.createUserTransactionCategory({ transactionCategoryUpsertRequest: view });
      case "organization":
        return await transactionCategoryService.createOrganizationTransactionCategory({
          organizationId: props.source.organizationId,
          transactionCategoryUpsertRequest: view
        });
      case "workspace":
        return await transactionCategoryService.createWorkspaceTransactionCategory({
          workspaceId: props.source.workspaceId,
          transactionCategoryUpsertRequest: view
        });
    }
  }

  function validate(view: Partial<TransactionCategoryUpsertRequest>): TransactionCategoryUpsertRequest | null {
    const { name, kind, ...other } = view;
    if (name == null || kind == null)
      return null;

    return { name, kind, ...other };
  }

  function setKind(kindStr: string) {
    setView({...view, kind: kindStr === "" ? undefined : kindStr as TransactionCategoryKind});
  }

  // TODO: Add color picker

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
      createHeader="Створення категорії транзакції"
      updateHeader="Редагування категорії транзакції"
    >
      <Grid xs={6}>
        <TextField
          label="Назва"
          required
          fullWidth
          value={view.name}
          onChange={e => setView({...view, name: e.target.value })}
        />
      </Grid>
      <Grid xs={6}>
        <FormControl fullWidth>
          <InputLabel>Тип</InputLabel>
          <Select
            label="Тип"
            required
            value={view.kind ?? ""}
            onChange={e => setKind(e.target.value)}
          >
            {Object.values(TransactionCategoryKind).map(value => (
              <MenuItem key={value} value={value}>{TransactionCategoryKind_i18[value]}</MenuItem>
            ))}
          </Select>
        </FormControl>
      </Grid>
      <Grid xs={12}>
        <Divider />
      </Grid>

    </UpsertComponent>
  );
}
