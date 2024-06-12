"use client";

import {Box, Link, Typography} from "@mui/material";

export default function DashboardPage() {
  return (
    <Box display="flex" flexDirection="column" height="100%" padding={2} boxSizing="border-box">
      <Typography variant="h4">
        Dashboard
      </Typography>
      <Link href="/workspaces">Workspaces</Link>
      <Link href="/organizations">Organizations</Link>
      <Link href="/profile">Profile</Link>
    </Box>
  );
}
