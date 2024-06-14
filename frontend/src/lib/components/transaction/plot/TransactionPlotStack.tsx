import {
  ListTransactionsRequest
} from "../../../../../generated";
import {useContext, useState} from "react";
import {LineChart} from "@mui/x-charts";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import TransactionPlot from "@/lib/components/transaction/plot/TransactionPlot";
import {cheerfulFiestaPalette} from "@mui/x-charts/colorPalettes";
import {TransactionPlotKind} from "@/lib/components/transaction/plot/Common";
import {LineChartGroupData, mapLineChartData} from "@/lib/utils/PlotUtils";


type TransactionPlotStackProps = {
  criteria: ListTransactionsRequest,
  kind: TransactionPlotKind,

  isDirty: boolean,
  setIsDirty: (value: boolean) => void
}

export default function TransactionPlotStack(props: TransactionPlotStackProps) {
  const [data, setData] = useState<LineChartGroupData>({
    totalAmount: 0,
    xAxis: [],
    series: []
  });
  const { transactionService } = useContext(ServicesContext);

  const stackStrategy = {
    stack: "total",
    area: true,
    stackOffset: "diverging",
    stackOrder: "descending",
    connectNulls: true
  } as const;

  async function fetch(): Promise<[number, LineChartGroupData]> {
    switch (props.kind.kind) {
      case "category": {
        const response = await transactionService.listTransactionsGroupByCategory({
          ...props.criteria,
          includeHistory: true
        });

        const chartData = mapLineChartData(response, "day", stackStrategy); // TODO
        console.log(JSON.stringify(chartData));

        return [chartData.totalAmount, chartData];
      }
      case "user": {
        const response = await transactionService.listTransactionsGroupByUser({
          ...props.criteria,
          includeHistory: true
        });
        const chartData = mapLineChartData(response, "day", stackStrategy); // TODO

        return [chartData.totalAmount, chartData];
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
      <LineChart
        xAxis={[{ data: data.xAxis, scaleType: "time" }]}
        series={data.series}
        height={500}
        colors={cheerfulFiestaPalette}
      />
    </TransactionPlot>
  );
}
