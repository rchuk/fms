import { AlertContext } from "@/lib/services/AlertService";
import { ServicesContext } from "@/lib/services/ServiceProvider";
import { getRequestError } from "@/lib/utils/RequestUtils";
import { Box, Button, Card, FormControl, TextField, Typography } from "@mui/material";
import { FormEvent, useContext, useState } from "react";

type ProfileUpdateCardProps = {
  prevFirstName: string,
  prevLastName: string,
  setEditorSelected: any
}

export default function ProfileUpdateCard({
  prevFirstName = "",
  prevLastName = "",
  setEditorSelected,
}: ProfileUpdateCardProps) {
  const [firstName, setFirstName] = useState(prevFirstName);
  const [firstNameError, setFirstNameError] = useState(false);
  const [lastName, setLastName] = useState(prevLastName);
  const [lastNameError, setLastNameError] = useState(false);
  const { userService } = useContext(ServicesContext);
  const showAlert = useContext(AlertContext);

  function onSubmit(event: FormEvent) {
    event.preventDefault();

    const isFirstNameValid = firstName.trim().length >= 3;
    const isLastNameValid = lastName.trim().length >= 3;

    setFirstNameError(!isFirstNameValid);
    setLastNameError(!isLastNameValid);

    if (isFirstNameValid && isLastNameValid) {
      userService.updateMe({
        userUpdate: {
          firstName,
          lastName
        }
      })
      .then(() => {
        setEditorSelected(false);
      })
      .catch((e) => {
        getRequestError(e).then(m => showAlert(m, "error"));
      });
    }
  }

  function onReset(event: FormEvent) {
    event.preventDefault();

    setEditorSelected(false);
  }

  return (
    <Card
      sx={{
        p: 4,
        pb: 1, 
        width: "30%",
        borderRadius: "2rem",
        "@media (max-width: 600px)": {
          p: 3,
          pb: 0.5,
          minWidth: "90%"
        }
      }}
    >
      <Typography 
        variant="h3"
        sx={{
          textAlign: "center"
        }}
      >
        Edit profile
      </Typography>
      <form
        style={{
          marginTop: "1.5rem",
          width: "100%"
        }}
        onSubmit={onSubmit}
        onReset={onReset}
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
              label="Fist name"
              required
              onChange={e => setFirstName(e.target.value)}
              type="text"
              value={firstName}
              error={firstNameError}
            />
          </FormControl>
          <FormControl
            fullWidth
            sx={{
              mt: 2
            }}
          >
            <TextField
              label="Last name"
              required
              onChange={e => setLastName(e.target.value)}
              type="text"
              value={lastName}
              error={lastNameError}
            />
          </FormControl>
          <Box
            sx={{
              mt: 3,
              width: "100%",
              display: "flex",
              flexDirection: "row",
              justifyContent: "space-evenly"
            }}
          >
            <Button 
              sx={{
                py: 1.5,
                width: "45%"
              }}
              type="submit"
            >
              <Typography variant="body1">
                Submit
              </Typography>
            </Button>
            <Button 
              sx={{
                py: 1.5,
                width: "45%"
              }}
              type="reset"
            >
              <Typography variant="body1">
                Back
              </Typography>
            </Button>
          </Box>
        </Box>
      </form>
    </Card>
  );
}