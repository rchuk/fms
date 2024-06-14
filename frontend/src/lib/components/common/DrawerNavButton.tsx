import { ListItem, ListItemButton, ListItemIcon, ListItemText, Typography } from "@mui/material";

type DrawerNavButtonProps = {
  href: string,
  Icon: any,
  text: string
}

export default function DrawerNavButton({
  href,
  Icon,
  text
}: DrawerNavButtonProps) {
  return (
    <ListItem disablePadding>
      <ListItemButton 
        href={href}
      >
        <ListItemIcon>
          <Icon 
            color="primary"
            fontSize="large"
          />
        </ListItemIcon>
        <ListItemText>
          <Typography
            variant="h5"
          >
            {text}
          </Typography>
        </ListItemText>
      </ListItemButton>
    </ListItem>
  )
}