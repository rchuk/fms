import { Box, Divider, List, ListItem, ListItemButton, ListItemIcon, ListItemText, Typography } from "@mui/material";
import { AccountCircleOutlined, CorporateFareOutlined, WorkspacesOutlined, SpaceDashboardOutlined, LoyaltyOutlined, CategoryOutlined, LogoutOutlined } from "@mui/icons-material";
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
        <ListItemText>
          <Typography 
            variant="h1" 
            fontFamily={"Helvetica"}
            sx={{
              py: 1,
              px: 2
            }}
          >
            FMS
          </Typography>
        </ListItemText>
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
            variant="h5" 
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