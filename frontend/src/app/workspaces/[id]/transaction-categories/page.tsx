import {Box} from "@mui/material";
import TransactionCategoryList from "@/lib/components/transaction-category/TransactionCategoryList";

export default function WorkspaceTransactionCategoriesPage({ params }: { params: { id: number } }) {
  return (
    <Box display="flex" flexDirection="column" height="100%" padding={2} boxSizing="border-box">
      <TransactionCategoryList source={{ kind: "workspace", workspaceId: params.id }} />
    </Box>
  );
}
