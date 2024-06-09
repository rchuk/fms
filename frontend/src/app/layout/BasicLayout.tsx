import {Box} from "@mui/material";
import {PropsWithChildren} from "react";

type BasicLayoutProps = {

};

export default function BasicLayout(props: PropsWithChildren<BasicLayoutProps>) {
  return (
    <Box sx={{ display: "flex", flexDirection: "column", height: "100%" }}>
      {props.children}
    </Box>
  );
}
