import { AppBar, Box, Drawer, IconButton, Toolbar, useTheme } from "@mui/material";
import { Menu } from "@mui/icons-material";
import { useState } from "react";
import DrawerNav from "./DrawerNav";
import {DRAWER_WIDTH} from "@/lib/utils/Constants";

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
    <Box sx={{ 
      display: 'flex',
      height: "100vh" 
    }}>
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
          width: { sm: DRAWER_WIDTH },
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
              width: DRAWER_WIDTH
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
              width: DRAWER_WIDTH
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
          display: "flex",
          flexDirection: "column",
          flexGrow: 1,
          p: 2,
          width: { 
            sm: `calc(100% - ${DRAWER_WIDTH}px)`
          }, 
          height: "100%"
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
        <Box
          sx={{
            flexGrow: 1
          }}
        >
          {children}
        </Box>
      </Box>
    </Box>
  );
}
