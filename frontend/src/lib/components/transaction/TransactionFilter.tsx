import {ListTransactionsRequest, TransactionCategoryKind, TransactionSortField} from "../../../../generated";
import {debounce, FormControl, InputLabel, MenuItem, Select, TextField} from "@mui/material";
import Grid from "@mui/material/Unstable_Grid2";
import {DateTimePicker} from "@mui/x-date-pickers";
import dayjs from "dayjs";
import {useMemo} from "react";
import {FILTER_DEBOUNCE_MS} from "@/lib/utils/Constants";
import TransactionCategoryAutocomplete from "@/lib/components/transaction-category/TransactionCategoryAutocomplete";
import SortDirectionSelect from "@/lib/components/common/SortDirectionSelect";
import {TransactionCategoryKind_i18} from "@/lib/i18/TransactionCategoryKind_i18";
import {TransactionSortField_i18} from "@/lib/i18/TransactionSortField_i18";


type TransactionFilterProps = {
  criteria: ListTransactionsRequest,
  setCriteria: (value: ListTransactionsRequest) => void,

  onChange: (criteria: ListTransactionsRequest) => void
}

export default function TransactionFilter(props: TransactionFilterProps) {
  function parseNumber(value: string): number | undefined {
    if (value.trim().length === 0)
      return undefined;

    return Number(value);
  }

  function setKind(value: string) {
    props.setCriteria({...props.criteria, categoryKind: value === "" ? undefined : value as TransactionCategoryKind});
  }

  function setSortField(value: string) {
    props.setCriteria({...props.criteria, sortField: value === "" ? undefined : value as TransactionSortField});
  }

  const searchDelayed = useMemo(
    () => debounce(() => props.onChange(props.criteria), FILTER_DEBOUNCE_MS),
    [props.criteria]
  );

  // TODO: Add missing user filter
  return (
    <Grid container columnSpacing={1} rowSpacing={2}>
      <Grid xs={3}>
        <TextField
          label="Мінімальна вартість"
          type="number"
          fullWidth
          value={props.criteria.minAmount}
          onChange={e => {
            props.setCriteria({...props.criteria, minAmount: parseNumber(e.target.value)});
            searchDelayed();
          }}
        />
      </Grid>
      <Grid xs={3}>
        <TextField
          label="Максимальна вартість"
          type="number"
          fullWidth
          value={props.criteria.maxAmount}
          onChange={e => {
            props.setCriteria({...props.criteria, maxAmount: parseNumber(e.target.value)})
            searchDelayed();
          }}
        />
      </Grid>
      <Grid xs={3}>
        <DateTimePicker
          label="Від"
          disableFuture
          slotProps={{
            actionBar: { actions: ["clear"], hidden: false },
            textField: { fullWidth: true }
          }}
          value={props.criteria.startDate != null ? dayjs(props.criteria.startDate) : null}
          onChange={value => {
            props.setCriteria({...props.criteria, startDate: value?.toDate() ?? undefined});
            searchDelayed();
          }}
        />
      </Grid>
      <Grid xs={3}>
        <DateTimePicker
          label="До"
          disableFuture
          slotProps={{
            actionBar: { actions: ["clear"], hidden: false },
            textField: { fullWidth: true }
          }}
          value={props.criteria.endDate != null ? dayjs(props.criteria.endDate) : null}
          onChange={value=> {
            props.setCriteria({...props.criteria, endDate: value?.toDate() ?? undefined});
            searchDelayed();
          }}
        />
      </Grid>
      <Grid xs={4}>
        <TransactionCategoryAutocomplete
          workspaceId={props.criteria.workspaceId}
          includeOwner={true}
          setSelectedId={(v) => {
            props.setCriteria({...props.criteria, categoryId: v ?? undefined });
            searchDelayed();
          }}
        />
      </Grid>
      <Grid xs={4}>
        <FormControl fullWidth>
          <InputLabel>Тип</InputLabel>
          <Select
            label="Тип"
            value={props.criteria.categoryKind ?? ""}
            onChange={e => {
              setKind(e.target.value);
              searchDelayed();
            }}
          >
            <MenuItem value="">Усі</MenuItem>
            {[TransactionCategoryKind.Income, TransactionCategoryKind.Expense].map(value => (
              <MenuItem key={value} value={value}>{TransactionCategoryKind_i18[value]}</MenuItem>
            ))}
          </Select>
        </FormControl>
      </Grid>
      <Grid xs={4} sx={{ display: "flex" }}>
        <FormControl fullWidth>
          <InputLabel>Сортування</InputLabel>
          <Select
            label="Сортування"
            value={props.criteria.sortField ?? ""}
            onChange={e => {
              setSortField(e.target.value);
              searchDelayed();
            }}
          >
            {Object.values(TransactionSortField).map(value => (
              <MenuItem key={value} value={value}>{TransactionSortField_i18[value]}</MenuItem>
            ))}
          </Select>
        </FormControl>
        <SortDirectionSelect criteria={props.criteria} setCriteria={props.setCriteria} searchDelayed={searchDelayed} />
      </Grid>
    </Grid>
  );
}
