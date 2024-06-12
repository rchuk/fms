import {FormEvent, useState} from "react";
import {Box, Button, FormControl, Link, TextField} from "@mui/material";

type CredentialsComponentProps = {
  handleSubmit: (email: string, password: string) => void
}

export default function RegisterForm(props: CredentialsComponentProps) {
  const [email, setEmail] = useState("");
  const [emailError, setEmailError] = useState(false);
  const [password, setPassword] = useState("");
  const [passwordError, setPasswordError] = useState(false);
  const [repeatPassword, setRepeatPassword] = useState("");
  const [repeatPasswordError, setRepeatPasswordError] = useState(false);

  function onSubmit(event: FormEvent) {
    event.preventDefault();

    const isEmailValid = email.trim().length >= 3;
    const isPasswordValid = password.trim().length >= 8;
    const isRepeatPasswordValid = password === repeatPassword;

    setEmailError(!isEmailValid);
    setPasswordError(!isPasswordValid);
    setRepeatPasswordError(!isRepeatPasswordValid);

    if (isEmailValid && isPasswordValid && isRepeatPasswordValid)
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
        <FormControl
          fullWidth
          sx={{
            mt: "1rem"
          }}
        >
          <TextField
            label="Repeat password"
            required
            onChange={e => setRepeatPassword(e.target.value)}
            type="password"
            value={repeatPassword}
            error={repeatPasswordError}
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
