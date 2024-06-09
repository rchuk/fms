"use client";

import {Typography} from "@mui/material";
import CredentialsComponent from "@/lib/components/auth/CredentialsComponent";

export default function RegisterPage() {
  function register(email: string, password: string) {
    console.log(email, password);
  }

  return (
    <main>
      <Typography variant="h3">
        Register
      </Typography>
      <CredentialsComponent handleSubmit={register}/>
    </main>
  );
}
