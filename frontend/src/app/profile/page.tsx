"use client";

import { Box } from "@mui/material";
import ProfileCard from "@/lib/components/profile/ProfileCard";

export default function ProfilePage() {
  return (
    <Box
      sx={{
        width: "100%",
        height: "100%",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        "@media (max-width: 600px)": {
          flexGrow: 1
        }
      }}
    >
      <ProfileCard />
    </Box>
  );
}