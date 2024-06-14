import {Box} from "@mui/material";
import WorkspaceList from "@/lib/components/workspace/WorkspaceList";

export default function UserWorkspacesPage() {
  return (
    <Box display="flex" flexDirection="column" height="100%" padding={2} boxSizing="border-box">
      <WorkspaceList source={{ kind: "user" }} />
    </Box>
  );
}
