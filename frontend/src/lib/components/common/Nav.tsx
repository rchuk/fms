import { AppBar, Box, Divider, Drawer, IconButton, Link, List, ListItem, ListItemButton, ListItemIcon, ListItemText, Toolbar, Typography, useTheme } from "@mui/material";
import { AccountCircleOutlined, CorporateFareOutlined, WorkspacesOutlined, SpaceDashboardOutlined, Menu } from "@mui/icons-material";
import { useState } from "react";
import DrawerNav from "./DrawerNav";

const drawerWidth = 300

type DrawerNavProps = {
  children: React.ReactNode
}

export default function Nav({
  children
}: DrawerNavProps) {
  const theme = useTheme();
  const [mobileOpen, setMobileOpen] = useState(false);
  const [isClosing, setIsClosing] = useState(false);

  function handleDrawerClose() {
    setIsClosing(true);
    setMobileOpen(false);
  };

  function handleDrawerTransitionEnd() {
    setIsClosing(false);
  };

  function handleDrawerToggle() {
    if (!isClosing) {
      setMobileOpen(!mobileOpen);
    }
  };

  return (
    <Box sx={{ display: 'flex' }}>
      <AppBar
        position="fixed"
        sx={{
          display: {
            sx: "block",
            sm: "none"
          }
        }}
      >
        <Toolbar>
          <IconButton
            color="primary"
            edge="start"
            onClick={handleDrawerToggle}
            sx={{ mr: 2, display: { sm: 'none' } }}
          >
            <Menu />
          </IconButton>
        </Toolbar>
      </AppBar>
      <Box
        component="nav"
        sx={{ 
          width: { sm: drawerWidth }, 
          flexShrink: { sm: 0 } 
        }}
      >
        <Drawer
          variant="temporary"
          open={mobileOpen}
          onTransitionEnd={handleDrawerTransitionEnd}
          onClose={handleDrawerClose}
          ModalProps={{
            keepMounted: true
          }}
          sx={{
            display: { 
              xs: 'block', 
              sm: 'none' 
            },
            '& .MuiDrawer-paper': { 
              boxSizing: 'border-box', 
              width: drawerWidth 
            }
          }}
        >
          <DrawerNav />
        </Drawer>
        <Drawer
          variant="permanent"
          sx={{
            display: { 
              xs: 'none', 
              sm: 'block' 
            },
            '& .MuiDrawer-paper': { 
              boxSizing: 'border-box', 
              width: drawerWidth 
            }
          }}
          open
        >
          <DrawerNav />
        </Drawer>
      </Box>
      <Box
        component="main"
        sx={{ 
          flexGrow: 1,
          p: 2,
          width: { 
            sm: `calc(100% - ${drawerWidth}px)` 
          } 
        }}
      >
        <Toolbar 
          sx={{ 
            display: {
              xs: "block",
              sm: "none"
            }
          }}
        />
        {children}
      </Box>
    </Box>
  );
}