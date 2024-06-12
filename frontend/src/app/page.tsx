"use client";

import { Box, Button, Container, Link, Stack, Typography } from "@mui/material";
import Grid from "@mui/material/Unstable_Grid2";
import { useRouter } from "next/navigation";

export default function Home() {
  const router = useRouter();
  
  return (
    <Box 
      overflow="hidden" 
      height={"100vh"} 
      display={"flex"}
      flexDirection={"column"} 
      justifyContent={"center"}
    >
      <Typography
        variant={"h1"} 
        align={"center"}
      >
        Welcome to FMS
      </Typography>
      <Typography
        variant={"h4"}
        align={"center"}
      >
        FMS is the next leader in finance management 
      </Typography>
      <Stack
        mt={10}
        sx={{          
          justifyContent: "space-evenly",
          alignItems: "center",
          flexDirection: "row",
          "@media (max-width: 600px)": {
            height: "13rem",
            flexDirection: "column"
          }
        }}
      >
        <Button 
          size={"large"}
          variant={"outlined"}
          sx={{
            fontSize: "2rem",
            width: "30vw",
            "@media (max-width: 600px)": {
              width: "80vw"
            }
          }}
          onClick={() => router.push("/auth/login")}
        >
          Login
        </Button>
        <Button 
          size={"large"}
          variant={"outlined"}
          sx={{
            fontSize: "2rem",
            width: "30vw",
            "@media (max-width: 600px)": {
              width: "80vw"
            }
          }}
          onClick={() => router.push("/auth/register")}
        >
          Register
        </Button>
      </Stack>
    </Box>
  );
}
