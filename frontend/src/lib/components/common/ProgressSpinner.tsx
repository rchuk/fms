import {Box, CircularProgress} from "@mui/material";
import React from "react";

export default function ProgressSpinner() {
  return (
    <Box display="flex" justifyContent="center" alignItems="center" height="100%">
      <CircularProgress />
    </Box>
  );
}
