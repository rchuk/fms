"use client";

import { Typography } from "@mui/material";
import CredentialsComponent from "@/lib/components/auth/CredentialsComponent";

export default function LoginPage() {
  function login(email: string, password: string) {
    console.log(email, password);
  }

  return (
    <main>
      <Typography variant="h3">
        Login
      </Typography>
      <CredentialsComponent handleSubmit={login}/>
    </main>
  );
}
