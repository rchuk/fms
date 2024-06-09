import {FormEvent, useState} from "react";
import {Button, FormControl, Link, TextField} from "@mui/material";

type CredentialsComponentProps = {
  handleSubmit: (email: string, password: string) => void
}

export default function CredentialsComponent(props: CredentialsComponentProps) {
  const [email, setEmail] = useState("");
  const [emailError, setEmailError] = useState(false);
  const [password, setPassword] = useState("");
  const [passwordError, setPasswordError] = useState(false);

  function onSubmit(event: FormEvent) {
    event.preventDefault();

    const isEmailValid = email.trim() !== "";
    const isPasswordValid = email.trim() !== "";

    setEmailError(!isEmailValid);
    setPasswordError(!isPasswordValid);

    if (isEmailValid && isPasswordValid)
      props.handleSubmit(email, password);
  }

  return (
    <form onSubmit={onSubmit}>
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
  );
}
