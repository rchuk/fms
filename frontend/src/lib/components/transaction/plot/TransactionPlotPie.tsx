import {DefaultChartsLegend, PieChart} from "@mui/x-charts";
import {
  ListTransactionsRequest
} from "../../../../../generated";
import {PieValueType} from "@mui/x-charts";
import {cheerfulFiestaPalette} from '@mui/x-charts/colorPalettes';
import {useContext, useEffect, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import {getRequestError} from "@/lib/utils/RequestUtils";
import {AlertContext} from "@/lib/services/AlertService";
import {TransactionPlotKind} from "@/lib/components/transaction/plot/Common";
import {Box, Typography} from "@mui/material";


type TransactionPlotPieProps = {
  criteria: ListTransactionsRequest,
  kind: TransactionPlotKind,

  isDirty: boolean,
  setIsDirty: (value: boolean) => void
}

export default function TransactionPlotPie(props: TransactionPlotPieProps) {
  const [data, setData] = useState<PieValueType[]>([]);
  const [total, setTotal] = useState<number>(0);
  const { transactionService } = useContext(ServicesContext);
  const showAlert = useContext(AlertContext);
  // TODO: Add different chart types

  useEffect(() => {
    if (props.isDirty) {
      fetch();
      props.setIsDirty(false);
    }
  }, [props.isDirty]);

  async function fetchImpl(): Promise<[number, PieValueType[]]> {
    switch (props.kind.kind) {
      case "category": {
        const {totalAmount, items} = await transactionService.listTransactionsGroupByCategory({...props.criteria});
        return [totalAmount, items.map(item => {
          let value: PieValueType = {
            id: item.category.id,
            value: Math.abs(item.amount),
            label: item.category.name,
          };

          if (item.category.uiColor != null)
            value.color = `#${item.category.uiColor}`;

          return value;
        })];
      }
      case "user": {
        const {totalAmount, items} = await transactionService.listTransactionsGroupByUser({...props.criteria});
        return [totalAmount, items.map(item => {
          return {
            id: item.user.id,
            value: Math.abs(item.amount),
            label: `${item.user.lastName} ${item.user.lastName}`
          };
        })];
      }
    }
  }

  function fetch() {
    const fetch = async() => {
       const [newTotal, newData] = await fetchImpl();
       setData(newData);
       setTotal(newTotal);
    }

    fetch().catch(e => getRequestError(e).then(m => showAlert(m, "error")));
  }

  return (
    <Box display="flex" flexDirection="column">
      <PieChart
        series={[ { data } ]}
        height={500}
        colors={cheerfulFiestaPalette}
      />
      <Typography variant="h6">
        Всього: {total}
      </Typography>
    </Box>
  );
}
