"use client";

import { Button, FormControl, Input, InputLabel, Link, TextField } from "@mui/material";
import { FormEvent, useState } from "react";

export default function LoginPage() {
  const [email, setEmail] = useState("");
  const [emailError, setEmailError] = useState(false);
  const [password, setPassword] = useState("");
  const [passwordError, setPasswordError] = useState(false);

  const handleSubmit = (event: FormEvent) => {
    event.preventDefault();

    setEmailError(false);
    setPasswordError(false);

    if (email == '')
      setEmailError(true);

    if (password == '')
      setPasswordError(true);

    if (email && password)
      console.log(email, password);
  };

  return (
    <main>
      <h1>
        Login
      </h1>
      <form onSubmit={handleSubmit}>
        <FormControl>
          <TextField 
            label="Email" 
            required
            onChange={e => setEmail(e.target.value)}
            type="email" 
            value={email}
            error={emailError} 
          />
        </FormControl>
        <FormControl>
          <TextField 
            label="Password" 
            required
            onChange={e => setPassword(e.target.value)}
            type="password" 
            value={password} 
            error={passwordError}
          />
        </FormControl>
        <Button type="submit">
          Submit
        </Button>
        <div>
          Need an account? <Link href="/auth/register">Register here</Link>
        </div>
      </form>
    </main>
  );
}