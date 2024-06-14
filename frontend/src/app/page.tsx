"use client";

import { Box, Button, Typography } from "@mui/material";
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
      <Box
        mt={10}
        sx={{    
          display: "flex",     
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
          variant={"outlined"}
          sx={{
            py: 2,
            width: "30vw",
            "@media (max-width: 600px)": {
              width: "80vw"
            }
          }}
          onClick={() => router.push("/auth/login")}
        >
          <Typography variant="h3">
            Login
          </Typography>
        </Button>
        <Button 
          variant={"outlined"}
          sx={{
            py: 2,
            width: "30vw",
            "@media (max-width: 600px)": {
              width: "80vw"
            }
          }}
          onClick={() => router.push("/auth/register")}
        >
          <Typography variant="h3">
            Register
          </Typography>
        </Button>
      </Box>
    </Box>
  );
}
