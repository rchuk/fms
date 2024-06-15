import {Box, IconButton, Tab, Tabs, Typography} from "@mui/material";
import {useState} from "react";
import EditIcon from '@mui/icons-material/Edit';
import VisibilityIcon from '@mui/icons-material/Visibility';


type PageTabsProps = {
  initialTab?: NavigationTabKind,
  header: string,
  canEdit: boolean,

  navigate: (value: NavigationTabKind) => void,
  mainLabel: string
}

export type NavigationTabKind = "Main" | "TransactionCategories" | "Users";

export default function PageTabs(props: PageTabsProps) {
  const [page, setPage] = useState<NavigationTabKind>(props.initialTab ?? "Main");

  function handleChange(newValue: NavigationTabKind) {
    setPage(newValue);

    props.navigate(newValue);
  }

  return (
    <Box>
      <Box display="flex" padding={2} justifyContent="space-between">
        <Typography variant="h3">
          {props.header}
        </Typography>
        <Box display="flex" justifyContent="flex-end" alignItems="center">
          <IconButton>
            {
              props.canEdit
                ? <EditIcon />
                : <VisibilityIcon />
            }
          </IconButton>
        </Box>
      </Box>
      <Tabs
        variant="fullWidth"
        value={page}
        onChange={(_, v) => handleChange(v as NavigationTabKind)}
      >
        <Tab label={props.mainLabel} value="Main" />
        <Tab label="Категорії транзакцій" value="TransactionCategories" />
        <Tab label="Користувачі" value="Users" />
      </Tabs>
    </Box>
  );
}
