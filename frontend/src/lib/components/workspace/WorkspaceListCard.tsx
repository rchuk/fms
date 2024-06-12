"use client";

import {WorkspaceKind, WorkspaceResponse, WorkspaceRole} from "../../../../generated";
import {Box, Card, CardActionArea, Typography} from "@mui/material";
import LockIcon from '@mui/icons-material/Lock';
import EditIcon from '@mui/icons-material/Edit';
import VisibilityIcon from '@mui/icons-material/Visibility';
import BackHandIcon from '@mui/icons-material/BackHand';
import AdminPanelSettingsIcon from '@mui/icons-material/AdminPanelSettings';
import {ReactElement} from "react";

type WorkspaceListCardProps = {
  item: WorkspaceResponse
}

export default function WorkspaceListCard(props: WorkspaceListCardProps) {
  const icons: Record<WorkspaceRole, () => ReactElement> = {
    [WorkspaceRole.Owner]: () => <BackHandIcon />,
    [WorkspaceRole.Admin]: () => <AdminPanelSettingsIcon />,
    [WorkspaceRole.Collaborator]: () => <EditIcon />,
    [WorkspaceRole.Viewer]: () => <VisibilityIcon />,
  };

  return (
    <Card key={props.item.id} variant="elevation" elevation={4}>
      <CardActionArea sx={{ padding: 2, display: "flex", alignItems: "center" }} href={`/workspaces/${props.item.id}`}>
        <Typography variant="h6">
          {props.item.name}
        </Typography>
        <Box display="flex" flex={1} justifyContent="flex-end">
          {props.item.kind == WorkspaceKind.Private && <LockIcon />}
          {icons[props.item.role]()}
        </Box>
      </CardActionArea>
    </Card>
  );
}
