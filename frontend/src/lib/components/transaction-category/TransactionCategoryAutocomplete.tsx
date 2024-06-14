import {useContext} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import AutocompleteComponent from "@/lib/components/common/AutocompleteComponent";
import {TransactionCategoryResponse} from "../../../../generated";


type TransactionCategoryAutocompleteProps = {
  initialId?: number,

  required?: boolean,
  disabled?: boolean,

  selectedCategory?: TransactionCategoryResponse | null,
  setSelectedCategory?: (value: TransactionCategoryResponse | null) => void,

  workspaceId: number,
  includeOwner?: boolean,
  setSelectedId: (value: number | null) => void
};

export default function TransactionCategoryAutocomplete(props: TransactionCategoryAutocompleteProps) {
  const { transactionCategoryService } = useContext(ServicesContext);

  async function fetch(query: string, count: number) {
    return await transactionCategoryService.listWorkspaceTransactionCategories({
      workspaceId: props.workspaceId,
      includeOwner: props.includeOwner,
      limit: count,
      query
    });
  }

  return (
    <AutocompleteComponent
      disabled={props.disabled}
      initialId={props.initialId}
      required={props.required}
      selectedItem={props.selectedCategory}
      setSelectedItem={props.setSelectedCategory}
      setSelectedId={props.setSelectedId}
      fetch={fetch}
      label={"Категорія траназакцій"}
      getItemLabel={(item: TransactionCategoryResponse) => item.name}
    />
  );
}
