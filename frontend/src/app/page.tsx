"use client";

import {Box, Button} from "@mui/material";
import styles from "./page.module.css";
import { useRouter } from "next/navigation";
import WorkspaceList from "@/lib/components/workspace/WorkspaceList";

export default function Home() {
  const router = useRouter();
  
  return (
    <Box display="flex" flexDirection="column" height="100%" padding={2} boxSizing="border-box">
      <div>
        Welcome to FMS
      </div>
      <Button onClick={() => router.push("/auth/login")}>
        Login
      </Button>
      <Button onClick={() => router.push("/auth/register")}>
        Register
      </Button>

      <WorkspaceList />
    </Box>
  );
}
