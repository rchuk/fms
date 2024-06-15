"use client";

import {TransactionResponse} from "../../../../generated";
import {Box, Card, CardActionArea, Typography, useTheme} from "@mui/material";
import ColorCircle from "@/lib/components/common/ColorCircle";

type TransactionListCardProps = {
  item: TransactionResponse,

  onClick: (id: number) => void
}

export default function TransactionListCard(props: TransactionListCardProps) {
  const theme = useTheme();
  const amountColor = props.item.amount < 0 ? theme.palette.error : theme.palette.success;

  return (
    <Card key={props.item.id} variant="elevation" elevation={4}>
      <CardActionArea sx={{ padding: 2, display: "flex", alignItems: "center" }} onClick={() => props.onClick(props.item.id)}>
        <ColorCircle color={props.item.category.uiColor} />
        <Typography variant="h6" marginLeft={2}>
          {props.item.category.name}
        </Typography>
        <Box display="flex" flex={1}>
          <Box flex={1}></Box>
          <Typography variant="h6" color={amountColor.main}>{props.item.amount}</Typography>
        </Box>
      </CardActionArea>
    </Card>
  );
}
