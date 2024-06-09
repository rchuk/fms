"use client";

import { Button, FormControl, Input, InputLabel, Link, TextField } from "@mui/material";
import { FormEvent, useState } from "react";

export default function RegisterPage() {
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
        Register
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
      </form>
      <div>
        Already have an account? <Link href="/auth/login">Login here</Link>
      </div>
    </main>
  );
}