import {Box} from "@mui/material";
import TransactionList from "@/lib/components/transaction/TransactionList";

export default function WorkspacePage({ params }: { params: { id: number } }) {
  return (
    <Box display="flex" flexDirection="column" height="100%" padding={2} boxSizing="border-box">
      <TransactionList workspaceId={params.id} />
    </Box>
  );
}
