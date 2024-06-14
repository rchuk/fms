"use client";

import {TransactionResponse} from "../../../../generated";
import {Box, Card, CardActionArea, Typography} from "@mui/material";
import ColorCircle from "@/lib/components/common/ColorCircle";

type TransactionListCardProps = {
  item: TransactionResponse
}

export default function TransactionListCard(props: TransactionListCardProps) {
  return (
    <Card key={props.item.id} variant="elevation" elevation={4}>
      <CardActionArea sx={{ padding: 2, display: "flex", alignItems: "center" }}>
        <ColorCircle color={props.item.category.uiColor} />
        <Typography variant="h6" marginLeft={2}>
          {props.item.category.name}
        </Typography>
        <Box display="flex" flex={1}>
          <Box flex={1}></Box>
          <Typography variant="h6">{props.item.amount}</Typography>
        </Box>
      </CardActionArea>
    </Card>
  );
}
