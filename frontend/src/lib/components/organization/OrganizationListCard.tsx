"use client";

import {OrganizationResponse} from "../../../../generated";
import {Box, Card, CardActionArea, Typography} from "@mui/material";

type OrganizationListCardProps = {
  item: OrganizationResponse
}

export default function OrganizationListCard(props: OrganizationListCardProps) {
  return (
    <Card key={props.item.id} variant="elevation" elevation={4}>
      <CardActionArea sx={{ padding: 2, display: "flex", alignItems: "center" }} href={`/organizations/${props.item.id}`}>
        <Typography variant="h6">
          {props.item.name}
        </Typography>
        <Box display="flex" flex={1} justifyContent="flex-end">

        </Box>
      </CardActionArea>
    </Card>
  );
}
