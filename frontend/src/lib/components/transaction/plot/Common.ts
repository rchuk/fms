

export interface TransactionPlotKindCategory {
  kind: "category"
}

export interface TransactionPlotKindUser {
  kind: "user"
}

export type TransactionPlotKind = TransactionPlotKindCategory | TransactionPlotKindUser;
