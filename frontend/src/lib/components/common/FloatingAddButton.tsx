import AddIcon from "@mui/icons-material/Add";
import {Fab} from "@mui/material";

type FloatingAddButtonProps = {
  onClick?: () => void
}

export default function FloatingAddButton(props: FloatingAddButtonProps) {
  return (
    <Fab
      color="primary"
      sx={{
        position: "fixed",
        bottom: 25,
        right: 25
      }}
      onClick={props.onClick}
    >
      <AddIcon />
    </Fab>
  );
}
