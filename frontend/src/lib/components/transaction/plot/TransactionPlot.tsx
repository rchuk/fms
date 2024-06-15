import {PropsWithChildren, useContext, useEffect, useState} from "react";
import {AlertContext} from "@/lib/services/AlertService";
import {getRequestError} from "@/lib/utils/RequestUtils";
import {Box, FormControl, InputLabel, MenuItem, Select, Typography} from "@mui/material";
import {TransactionPlotKind} from "@/lib/components/transaction/plot/Common";


interface TransactionPlotProps<RenderDataT> {
  data: RenderDataT,
  setData: (value: RenderDataT) => void,

  kind: TransactionPlotKind,
  setKind: (value: TransactionPlotKind) => void,

  fetch: () => Promise<[number, RenderDataT]>,

  total?: number,
  setTotal?: (value: number) => void,

  isDirty: boolean,
  setIsDirty: (value: boolean) => void
}

export default function TransactionPlot<RenderDataT>(props: PropsWithChildren<TransactionPlotProps<RenderDataT>>) {
  const [totalInternal, setTotalInternal] = useState<number>(0);
  const showAlert = useContext(AlertContext);

  const [total, setTotal] = [props.total ?? totalInternal, props.setTotal ?? setTotalInternal];

  useEffect(() => {
    if (props.isDirty) {
      fetch();
      props.setIsDirty(false);
    }
  }, [props.isDirty]);

  function fetch() {
    const fetch = async() => {
      const [newTotal, newData] = await props.fetch();
      props.setData(newData);
      setTotal(newTotal);
    }

    fetch().catch(e => getRequestError(e).then(m => showAlert(m, "error")));
  }

  return (
    <Box display="flex" flexDirection="column" marginTop={4}>
      {props.children}
      <Box display="flex" justifyContent="space-between" alignItems="end">
        <Typography variant="h6">
          Всього: {total}
        </Typography>
        <FormControl>
          <InputLabel>Групування</InputLabel>
          <Select
            label="Групування"
            value={props.kind}
            onChange={e => props.setKind(e.target.value as TransactionPlotKind)}
            sx={{ width: 200 }}
          >
            <MenuItem value={"category"}>Категорія</MenuItem>
            <MenuItem value={"user"}>Користувач</MenuItem>
          </Select>
        </FormControl>
      </Box>
    </Box>
  );
}
