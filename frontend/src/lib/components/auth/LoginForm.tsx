import {FormEvent, useState} from "react";
import {Box, Button, FormControl, Link, TextField} from "@mui/material";

type CredentialsComponentProps = {
  handleSubmit: (email: string, password: string) => void
}

export default function LoginForm(props: CredentialsComponentProps) {
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
    <form
      style={{
        width: "100%"
      }}
      onSubmit={onSubmit}
    >
      <Box
        display={"flex"}
        flexDirection={"column"}
        justifyContent={"space-evenly"}
        alignItems={"center"}
      >
        <FormControl
          fullWidth
        >
          <TextField
            label="Email"
            required
            onChange={e => setEmail(e.target.value)}
            type="email"
            value={email}
            error={emailError}
          />
        </FormControl>
        <FormControl
          fullWidth
          sx={{
            mt: "1rem"
          }}
        >
          <TextField
            label="Password"
            required
            onChange={e => setPassword(e.target.value)}
            type="password"
            value={password}
            error={passwordError}
          />
        </FormControl>
        <Button 
          sx={{
            mt: "1.5rem",
            fontSize: "1.2rem",
            width: "100%"
          }}
          type="submit"
        >
          Submit
        </Button>
      </Box>
    </form>
  );
}
