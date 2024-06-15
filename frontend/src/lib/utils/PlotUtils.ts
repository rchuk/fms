import {
  TransactionGroupedByCategoryListResponse, TransactionGroupedByCategoryResponse,
  TransactionGroupedByUserListResponse, TransactionGroupedByUserResponse,
  TransactionShortResponse
} from "../../../generated";
import dayjs from "dayjs";
import {LineSeriesType} from "@mui/x-charts";

interface LineChartEntry {
  date: Date,
  sum: number
}

export interface LineChartGroupData {
  totalAmount: number,
  xAxis: Date[],
  series: LineSeriesType[]
}

export function mapLineChartData(response: TransactionGroupedByCategoryListResponse | TransactionGroupedByUserListResponse,
                          duration: dayjs.UnitType & dayjs.ManipulateType,
                          stack: Partial<LineSeriesType>): LineChartGroupData {
  const grouped = response.items.map(group => {
    const label = getGroupLabel(group);
    const series = groupTransactionsByTime(group.history ?? [], duration);

    return {
      label,
      series
    };
  });
  const [minDate, maxDate] = findMinMaxDate(grouped.map(group => group.series));
  const continuous = grouped.map(group => {
    return {
      label: group.label,
      series: fillDateHoles(group.series, minDate, maxDate, duration)
    };
  });

  const series = continuous.map(group => {
    return {
      type: "line",
      label: group.label,
      data: group.series.map(entry => entry?.sum),
      ...stack
    } as LineSeriesType;
  })

  return {
    totalAmount: response.totalAmount,
    xAxis: generateAxisArray(minDate, maxDate, duration),
    series
  };
}

function getGroupLabel(group: TransactionGroupedByCategoryResponse | TransactionGroupedByUserResponse): string {
  function isGroupedByCategory(group: any): group is TransactionGroupedByCategoryResponse {
    return (group as TransactionGroupedByCategoryResponse).category !== undefined;
  }

  function isGroupedByUser(group: any): group is TransactionGroupedByUserResponse {
    return (group as TransactionGroupedByUserResponse).user !== undefined;
  }

  if (isGroupedByCategory(group)) {
    return group.category.name;
  } else if (isGroupedByUser(group)) {
    return `${group.user.firstName} ${group.user.lastName}`;
  } else {
    return "";
  }
}

function generateAxisArray(
  startDate: Date,
  endDate: Date,
  duration: dayjs.ManipulateType
): Date[] {
  const rangeArray: Date[] = [];
  let currentDate = dayjs(startDate);

  while (currentDate.isBefore(endDate) || currentDate.isSame(endDate, duration)) {
    rangeArray.push(currentDate.toDate());
    currentDate = currentDate.add(1, duration);
  }

  return rangeArray;
}

function findMinMaxDate(data: LineChartEntry[][]): [Date, Date] {
  const dates = data.flat().map(item => dayjs(item.date).toDate().getTime());

  return [new Date(Math.min(...dates)), new Date(Math.max(...dates))];
}

function groupTransactionsByTime(data: TransactionShortResponse[], duration: dayjs.UnitType): LineChartEntry[] {
  const groupedData = Object.groupBy(data, item => dayjs(item.timestamp).get(duration));

  return Object.entries(groupedData)
    .map(([_, items]) => items)
    .filter(items => items !== undefined)
    .map(items => {
      const list = items as TransactionShortResponse[];
      const sum = list.reduce((acc, item) => acc + item.amount, 0) ?? 0;

      return { date: list[0].timestamp, sum };
    });
}

function fillDateHoles(data: LineChartEntry[], start: Date, end: Date, duration: dayjs.ManipulateType): (LineChartEntry | null)[] {
  const result: (LineChartEntry | null)[] = [];
  const dataMap = new Map(data.map(item => [dayjs(item.date).toISOString(), item]));

  let currentDate = dayjs(start);

  while (currentDate.isBefore(end) || currentDate.isSame(end, duration)) {
    const dateStr = currentDate.toISOString();
    result.push(dataMap.get(dateStr) || null);
    currentDate = currentDate.add(1, duration);
  }

  return result;
}
