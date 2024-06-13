import {PieChart} from "@mui/x-charts";
import {TransactionResponse} from "../../../../generated";


type TransactionPlotProps = {
  itemsData: TransactionResponse[]
}

export default function TransactionPlot(props: TransactionPlotProps) {
  // TODO: Add different chart types

  function mapItem(item: TransactionResponse) {
    return {
      id: item.id,
      value: Math.abs(item.amount),
      label: item.category.name
    }
  };

  return (
    <PieChart
      series={[ { data: props.itemsData.map(mapItem) } ]}
      height={500}
    />
  );
}
