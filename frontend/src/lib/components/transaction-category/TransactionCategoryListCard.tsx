"use client";

import {TransactionCategoryKind, TransactionCategoryResponse} from "../../../../generated";
import {Box, Card, CardActionArea, Typography} from "@mui/material";

type TransactionCategoryListCardProps = {
  item: TransactionCategoryResponse
}

export default function TransactionCategoryListCard(props: TransactionCategoryListCardProps) {
  // TODO: Add i18 type
  const kindText: Record<TransactionCategoryKind, string> = {
    [TransactionCategoryKind.Income]: "Дохід",
    [TransactionCategoryKind.Expense]: "Витрати",
    [TransactionCategoryKind.Mixed]: "Змішаний"
  };

  return (
    <Card key={props.item.id} variant="elevation" elevation={4}>
      <CardActionArea sx={{ padding: 2, display: "flex", alignItems: "center" }}>
        <Typography variant="h6">
          {props.item.name}
        </Typography>
        <Box display="flex" flex={1} justifyContent="flex-end">
          <Typography variant="h6">{kindText[props.item.kind]}</Typography>
        </Box>
      </CardActionArea>
    </Card>
  );
}
