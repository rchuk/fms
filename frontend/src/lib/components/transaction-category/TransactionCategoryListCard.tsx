"use client";

import {TransactionCategoryKind, TransactionCategoryResponse} from "../../../../generated";
import {Box, Card, CardActionArea, Typography, useTheme} from "@mui/material";
import ColorCircle from "@/lib/components/common/ColorCircle";
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import RemoveCircleOutlineIcon from '@mui/icons-material/RemoveCircleOutline';
import AdjustIcon from '@mui/icons-material/Adjust';
import {ReactElement} from "react";

type TransactionCategoryListCardProps = {
  item: TransactionCategoryResponse,

  onClick?: (id: number) => void
}

export default function TransactionCategoryListCard(props: TransactionCategoryListCardProps) {
  const theme = useTheme();
  const iconStyles: Record<TransactionCategoryKind, [(props: any) => ReactElement, string | undefined]> = {
    [TransactionCategoryKind.Income]: [
      (props) => <AddCircleOutlineIcon {...props} />,
      theme.palette.success.main
    ],
    [TransactionCategoryKind.Expense]: [
      (props) => <RemoveCircleOutlineIcon {...props} />,
      theme.palette.error.main
    ],
    [TransactionCategoryKind.Mixed]: [
      (props) => <AdjustIcon {...props} />,
      undefined
    ]
  };

  const [icon, color] = iconStyles[props.item.kind];

  return (
    <Card key={props.item.id} variant="elevation" elevation={4}>
      <CardActionArea sx={{ padding: 2, display: "flex", alignItems: "center" }} onClick={() => props.onClick?.(props.item.id)}>
        <ColorCircle color={props.item.uiColor} />
        <Typography variant="h6" marginLeft={2}>
          {props.item.name}
        </Typography>
        <Box display="flex" flex={1}>
          <Box flex={1}></Box>
          <Typography color={color} display="flex">{icon({ fontSize: "large" })}</Typography>
        </Box>
      </CardActionArea>
    </Card>
  );
}
