import { Box, Button, Divider, List, ListItem, ListItemButton, ListItemIcon, ListItemText, Typography } from "@mui/material";
import { AccountCircleOutlined, CorporateFareOutlined, WorkspacesOutlined, SpaceDashboardOutlined, LoyaltyOutlined, CategoryOutlined, LogoutOutlined } from "@mui/icons-material";
import Image from "next/image";
import DrawerNavButton from "./DrawerNavButton";
import { useContext } from "react";
import { SessionServiceContext } from "@/lib/services/SessionService";
import { useRouter } from "next/navigation";


export default function DrawerNav() {
  const sessionData = useContext(SessionServiceContext);
  const router = useRouter();
  function logout() {
    sessionData.setAccessToken(null);
    router.push("/");
  }

  return (
    <List
      sx={{
        display: "flex",
        flexDirection: "column",
        justifyContent: "flex-start",
        height: "100%",
        py: 2
      }}
    >
      <ListItem disablePadding>
        <Box display="flex" flexGrow={1} justifyContent="center">
          <Button disableRipple sx={{ padding: 0 }} onClick={() => router.push("/dashboard")}>
            <Image src="fms_logo.svg" width={200} height={80} alt="FMS" priority />
          </Button>
        </Box>
      </ListItem>
      <Divider />
      <DrawerNavButton 
        href="/profile"
        Icon={AccountCircleOutlined}
        text="Profile"
      />
      <DrawerNavButton 
        href="/subscriptions"
        Icon={LoyaltyOutlined}
        text="Subscriptions"
      />
      <DrawerNavButton 
        href="/dashboard"
        Icon={SpaceDashboardOutlined}
        text="Dashboard"
      />
      <DrawerNavButton 
        href="/organizations"
        Icon={CorporateFareOutlined}
        text="Organizations"
      />
      <DrawerNavButton 
        href="/workspaces"
        Icon={WorkspacesOutlined}
        text="Workspaces"
      />
      <DrawerNavButton 
        href="/transaction-categories"
        Icon={CategoryOutlined}
        text="Transaction categories"
      />
      
      <ListItem 
        disablePadding
        sx={{
          mt: "auto"
        }}
      >
      <ListItemButton 
        onClick={logout}
        
      >
        <ListItemIcon>
          <LogoutOutlined 
            color="error"
            fontSize="large"
          />
        </ListItemIcon>
        <ListItemText>
          <Typography
            color="error"
          >
            Logout
          </Typography>
        </ListItemText>
      </ListItemButton>
    </ListItem>
    </List>
  );
}
