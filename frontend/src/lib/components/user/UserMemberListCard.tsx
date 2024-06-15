"use client";

import {UserResponse} from "../../../../generated";
import {Box, Card, CardActionArea, Typography} from "@mui/material";
import {ReactElement} from "react";

type UserMemberListCardProps<RoleT> = {
  item: UserResponse,
  role: RoleT,

  renderRole: (role: RoleT) => ReactElement,

  onClick?: (item: UserResponse) => void
}

export default function UserMemberListCard<RoleT>(props: UserMemberListCardProps<RoleT>) {
  return (
    <Card key={props.item.id} variant="elevation" elevation={4}>
      <CardActionArea sx={{ padding: 2, display: "flex", alignItems: "center" }} onClick={() => props.onClick?.(props.item)}>
        <Typography variant="h6" marginLeft={2}>
          {props.item.firstName} {props.item.lastName}
        </Typography>
        <Box display="flex" flex={1}>
          <Box flex={1}></Box>
          <Typography variant="h6">{props.renderRole(props.role)}</Typography>
        </Box>
      </CardActionArea>
    </Card>
  );
}
