import {Box} from "@mui/material";
import TransactionCategoryList from "@/lib/components/transaction-category/TransactionCategoryList";

export default function UserTransactionCategoriesPage() {
  return (
    <Box display="flex" flexDirection="column" height="100%" padding={2} boxSizing="border-box">
      <TransactionCategoryList source={{ kind: "user" }} />
    </Box>
  );
}
