import React, {PropsWithChildren} from "react";
import {Box, Button, Typography} from "@mui/material";
import Grid from "@mui/material/Unstable_Grid2";


type UpsertContainerProps = {
  submit: () => void,
  cancel: () => void,
  header: string
};

export default function UpsertContainer(props: PropsWithChildren<UpsertContainerProps>): React.ReactNode {
  return (
    <Box
      component="form"
      onSubmit={e => {e.preventDefault(); props.submit()}}
      sx={{ width: 500 }}
    >
      <Grid container columnSpacing={1} rowSpacing={2}>
        <Grid xs={12} textAlign="center">
          <Typography variant="h4" sx={{ marginBottom: 4 }}>
            {props.header}
          </Typography>
        </Grid>
        {props.children}
        <Grid xs={12}>
          <Box display="flex" justifyContent="flex-end" columnGap={1}>
            <Button type="submit" variant="outlined">Зберегти</Button>
            <Button variant="outlined" onClick={props.cancel}>Скасувати</Button>
          </Box>
        </Grid>
      </Grid>
    </Box>
  )
}
