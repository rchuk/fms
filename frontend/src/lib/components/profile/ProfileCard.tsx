import { Button, Typography } from "@mui/material";
import EditOutlinedIcon from '@mui/icons-material/EditOutlined';
import UserCard from "./UserCard";
import { useContext, useEffect, useState } from "react";
import { AlertContext } from "@/lib/services/AlertService";
import { ServicesContext } from "@/lib/services/ServiceProvider";
import { getRequestError } from "@/lib/utils/RequestUtils";
import { UserSelfResponse } from "../../../../generated";
import ProfileUpdateCard from "./ProfileUpdateCard";

export default function ProfileCard() {
  const [editorSelected, setEditorSelected] = useState(false);
  const { authService } = useContext(ServicesContext);
  const showAlert = useContext(AlertContext)

  const [user, setUser] = useState<UserSelfResponse | null>(null);
  useEffect(() => {
    authService.getMe()
    .then((response: UserSelfResponse) => {
      setUser(response);
    })
    .catch((e) => {
      getRequestError(e).then(m => showAlert(m, "error"));
    });
  }, [editorSelected]);

  return (
      editorSelected ? (
        <ProfileUpdateCard 
          setEditorSelected={setEditorSelected}
          prevFirstName={user!.firstName}
          prevLastName={user!.lastName}
        />
      ) : (
        <UserCard {...user!}>
          <Button
            sx={{
              p: 1,
              mt: 3,
              width: "20rem",
              "@media (max-width: 600px)": {
                width: "100%"
              }
            }}
            onClick={() => setEditorSelected(true)}
          >
            <EditOutlinedIcon color="primary" fontSize="large" />
            <Typography variant="h5">
              &nbsp;Edit profile
            </Typography>
          </Button>
        </UserCard>
      )
  );
}