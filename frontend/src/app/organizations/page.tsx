import {Box} from "@mui/material";
import OrganizationList from "@/lib/components/organization/OrganizationList";

export default function UserOrganizationsPage() {
  return (
    <Box display="flex" flexDirection="column" height="100%" padding={2} boxSizing="border-box">
      <OrganizationList />
    </Box>
  );
}
