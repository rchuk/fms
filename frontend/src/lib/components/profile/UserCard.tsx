import { Avatar, Box, Button, Card, IconButton, Tooltip, Typography, emphasize } from "@mui/material";
import { stringToColor } from "@/lib/utils/AvatarUtils";

type UserCardProps = {
  children?: React.ReactNode,
  firstName: string,
  lastName: string,
  email: string | null
}

export default function UserCard({
  children,
  firstName = "",
  lastName = "",
  email = null
}: UserCardProps) {
  const color = stringToColor(firstName + " " + lastName);
  return (
    <Card
      sx={{
        p: 6,
        borderRadius: "2rem",
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        "@media (max-width: 600px)": {
          p: 2,
          minWidth: "90%"
        }
      }}
    >
      <Avatar 
        sx={{
          bgcolor: color,
          color: emphasize(color, 0.5),
          aspectRatio: 1,
          width: "16rem",
          height: "auto",
          "@media (max-width: 600px)": {
            width: "12rem"
          }
        }}
      >
        <Typography variant="h1">
          {firstName[0] + lastName[0]}
        </Typography>
      </Avatar>
      <Box
        sx={{
          mt: 4,
          display: "flex",
          flexDirection: "row",
          justifyContent: "center"
        }}
      >
        <Typography 
          variant="h2"
          textAlign={"center"}
        >
          {firstName} {lastName}
        </Typography>
      </Box>
      { email ? (
        <Typography
          variant="h5"
        >
          {email}
        </Typography>
      ) : []}
      {children}
    </Card>
  );
}