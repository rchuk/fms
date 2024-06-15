"use client";

import {OrganizationResponse, OrganizationRole, WorkspaceRole} from "../../../../generated";
import {Box, Card, CardActionArea, Typography} from "@mui/material";
import {ReactElement} from "react";
import BackHandIcon from "@mui/icons-material/BackHand";
import AdminPanelSettingsIcon from "@mui/icons-material/AdminPanelSettings";
import EditIcon from "@mui/icons-material/Edit";

type OrganizationListCardProps = {
  item: OrganizationResponse
}

export function renderOrganizationRole(role: OrganizationRole): () => ReactElement {
  const icons: Record<OrganizationRole, () => ReactElement> = {
    [OrganizationRole.Owner]: () => <BackHandIcon />,
    [OrganizationRole.Admin]: () => <AdminPanelSettingsIcon />,
    [OrganizationRole.Member]: () => <EditIcon />
  };

  return icons[role];
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
