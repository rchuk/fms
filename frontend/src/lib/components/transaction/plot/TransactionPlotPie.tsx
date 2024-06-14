import {PieChart} from "@mui/x-charts";
import {
  ListTransactionsRequest
} from "../../../../../generated";
import {PieValueType} from "@mui/x-charts";
import {cheerfulFiestaPalette} from '@mui/x-charts/colorPalettes';
import {useContext, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import {TransactionPlotKind} from "@/lib/components/transaction/plot/Common";
import TransactionPlot from "@/lib/components/transaction/plot/TransactionPlot";


type TransactionPlotPieProps = {
  criteria: ListTransactionsRequest,
  kind: TransactionPlotKind,

  isDirty: boolean,
  setIsDirty: (value: boolean) => void
}

export default function TransactionPlotPie(props: TransactionPlotPieProps) {
  const [data, setData] = useState<PieValueType[]>([]);
  const { transactionService } = useContext(ServicesContext);

  async function fetch(): Promise<[number, PieValueType[]]> {
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

  return (
    <TransactionPlot
      data={data}
      setData={setData}
      fetch={fetch}
      isDirty={props.isDirty}
      setIsDirty={props.setIsDirty}
    >
      <PieChart
        series={[ { data } ]}
        height={500}
        colors={cheerfulFiestaPalette}
      />
    </TransactionPlot>
  );
}
