"use client";

import {Box, Button, Link} from "@mui/material";
import { useRouter } from "next/navigation";

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

      <Link href="/workspaces">Workspaces</Link>
    </Box>
  );
}
