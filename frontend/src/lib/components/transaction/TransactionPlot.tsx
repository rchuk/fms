import {PieChart} from "@mui/x-charts";
import {
  ListTransactionsRequest,
  TransactionGroupedByCategoryResponse
} from "../../../../generated";
import {PieValueType} from "@mui/x-charts";
import {cheerfulFiestaPalette} from '@mui/x-charts/colorPalettes';
import {useContext, useEffect, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import {getRequestError} from "@/lib/utils/RequestUtils";
import {AlertContext} from "@/lib/services/AlertService";


type TransactionPlotProps = {
  criteria: ListTransactionsRequest
}

export default function TransactionPlot(props: TransactionPlotProps) {
  const [data, setData] = useState<TransactionGroupedByCategoryResponse[]>([]);
  const { transactionService } = useContext(ServicesContext);
  const showAlert = useContext(AlertContext);
  // TODO: Add different chart types
  // Added stacked chart
  // TODO: Add chart by user

  useEffect(() => {
    fetch();
  }, [props.criteria]);

  function fetch() {
    const fetch = async() => {
       const newData = await transactionService.listTransactionsGroupByCategory({...props.criteria});
       setData(newData);
    }

    fetch().catch(e => getRequestError(e).then(m => showAlert(m, "error")));
  }

  function mapItem(item: TransactionGroupedByCategoryResponse): PieValueType {
    return {
      id: item.category.id,
      value: Math.abs(item.amount),
      label: item.category.name,
      color: item.category.uiColor != null ? `#${item.category.uiColor}` : undefined
    };
  }

  return (
    <PieChart
      series={[ { data: data.map(mapItem) } ]}
      height={500}
      colors={cheerfulFiestaPalette}
    />
  );
}
