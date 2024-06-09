"use client";

import {Link, Typography} from "@mui/material";
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
      <div>
        Already have an account? <Link href="/auth/login">Login here</Link>
      </div>
    </main>
  );
}
