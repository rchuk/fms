"use client";

import {TransactionCategoryResponse} from "../../../../generated";
import {Box, Card, CardActionArea, Typography} from "@mui/material";
import {TransactionCategoryKind_i18} from "@/lib/i18/TransactionCategoryKind_i18";
import ColorCircle from "@/lib/components/common/ColorCircle";

type TransactionCategoryListCardProps = {
  item: TransactionCategoryResponse,

  onClick?: (id: number) => void
}

export default function TransactionCategoryListCard(props: TransactionCategoryListCardProps) {
  return (
    <Card key={props.item.id} variant="elevation" elevation={4}>
      <CardActionArea sx={{ padding: 2, display: "flex", alignItems: "center" }} onClick={() => props.onClick?.(props.item.id)}>
        <ColorCircle color={props.item.uiColor} />
        <Typography variant="h6" marginLeft={2}>
          {props.item.name}
        </Typography>
        <Box display="flex" flex={1}>
          <Box flex={1}></Box>
          <Typography variant="h6">{TransactionCategoryKind_i18[props.item.kind]}</Typography>
        </Box>
      </CardActionArea>
    </Card>
  );
}
