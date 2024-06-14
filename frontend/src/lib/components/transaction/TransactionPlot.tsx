import {PieChart} from "@mui/x-charts";
import {TransactionResponse} from "../../../../generated";
import {PieValueType} from "@mui/x-charts";
import {cheerfulFiestaPalette} from '@mui/x-charts/colorPalettes';


type TransactionPlotProps = {
  itemsData: TransactionResponse[]
}

export default function TransactionPlot(props: TransactionPlotProps) {
  // TODO: Add different chart types
  // Added stacked chart
  // TODO: Add chart by user

  function mapItem(item: TransactionResponse): PieValueType {
    return {
      id: item.id,
      value: Math.abs(item.amount),
      label: item.category.name,
      color: item.category.uiColor != null ? `#${item.category.uiColor}` : undefined
    };
  }

  return (
    <PieChart
      series={[ { data: props.itemsData.map(mapItem) } ]}
      height={500}
      colors={cheerfulFiestaPalette}
    />
  );
}
