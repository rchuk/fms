import {PieChart} from "@mui/x-charts";
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


type TransactionPlotPieProps = {
  criteria: ListTransactionsRequest,
  kind: TransactionPlotKind,

  isDirty: boolean,
  setIsDirty: (value: boolean) => void
}

export default function TransactionPlotPie(props: TransactionPlotPieProps) {
  const [data, setData] = useState<PieValueType[]>([]);
  const { transactionService } = useContext(ServicesContext);
  const showAlert = useContext(AlertContext);
  // TODO: Add different chart types

  useEffect(() => {
    if (props.isDirty) {
      fetch();
      props.setIsDirty(false);
    }
  }, [props.isDirty]);

  async function fetchImpl(): Promise<PieValueType[]> {
    switch (props.kind.kind) {
      case "category":
        const categoryResponses = await transactionService.listTransactionsGroupByCategory({...props.criteria});
        return categoryResponses.map(item => {
          let value: PieValueType = {
            id: item.category.id,
            value: Math.abs(item.amount),
            label: item.category.name,
          };

          if (item.category.uiColor != null)
            value.color = `#${item.category.uiColor}`;

          return value;
        });

      case "user":
        const userResponse = await transactionService.listTransactionsGroupByUser({...props.criteria});
        return userResponse.map(item => {
          return {
            id: item.user.id,
            value: Math.abs(item.amount),
            label: `${item.user.lastName} ${item.user.lastName}`
          };
        });
    }
  }

  function fetch() {
    const fetch = async() => {
       const newData = await fetchImpl();
       setData(newData);
    }

    fetch().catch(e => getRequestError(e).then(m => showAlert(m, "error")));
  }

  return (
    <PieChart
      series={[ { data } ]}
      height={500}
      colors={cheerfulFiestaPalette}
    />
  );
}
