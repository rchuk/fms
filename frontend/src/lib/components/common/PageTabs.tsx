import {Tab, Tabs} from "@mui/material";
import {useState} from "react";


type PageTabsProps = {
  initialTab?: NavigationTabKind,

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
    <Tabs
      variant="fullWidth"
      value={page}
      onChange={(_, v) => handleChange(v as NavigationTabKind)}
    >
      <Tab label={props.mainLabel} value="Main" />
      <Tab label="Категорії транзакцій" value="TransactionCategories" />
      <Tab label="Користувачі" value="Users" />
    </Tabs>
  );
}
