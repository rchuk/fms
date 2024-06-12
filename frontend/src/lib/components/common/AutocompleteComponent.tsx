"use client";

import {Autocomplete, debounce, TextField} from "@mui/material";
import {useContext, useEffect, useMemo, useState} from "react";
import {getRequestError} from "@/lib/utils/RequestUtils";
import {AlertContext} from "@/lib/services/AlertService";
import {BaseEntity, EntityId, findEntity, ListResponse} from "@/lib/utils/EntityUtils";
import {SEARCH_DEBOUNCE_MS} from "@/lib/utils/Constants";

type AutocompleteComponentProps<ItemT extends BaseEntity<IdT>, IdT extends EntityId> = {
  initialId?: IdT,
  required?: boolean,
  setSelectedId: (value: IdT | null) => void,

  fetch: (query: string, count: number) => Promise<ListResponse<ItemT>>,

  label: string,
  getItemLabel: (item: ItemT) => string,
  maxCount?: number,

  items?: ItemT[],
  setItems?: (value: ItemT[]) => void
};

export default function AutocompleteComponent<ItemT extends BaseEntity<IdT>, IdT extends EntityId>
(props: AutocompleteComponentProps<ItemT, IdT>) {
  const [internalItems, internalSetItems] = useState<ItemT[]>([]);
  const [inputValue, setInputValue] = useState<string>("");
  const [query, setQuery] = useState<string>("");
  const [selectedItem, setSelectedItem] = useState<ItemT | null>(null);
  const showAlert = useContext(AlertContext);

  const items = props.items ?? internalItems;
  const setItems = props.setItems ?? internalSetItems;

  useEffect(() => {
    if (props.initialId != null)
      setSelectedItem(findEntity(items, props.initialId));
  }, [props.initialId, items]);

  useEffect(() => {
    const fetch = async() => {
      const response = await props.fetch(query, props.maxCount ?? 10);

      setItems(response.items);
    };

    fetch().catch(e => getRequestError(e).then(m => showAlert(m, "error")));
  }, [query]);

  const searchDelayed = useMemo(
    () => debounce(() => setQuery(inputValue.trim()), SEARCH_DEBOUNCE_MS),
    [inputValue]
  );

  return (
    <Autocomplete
      disablePortal
      fullWidth
      options={items}
      getOptionLabel={props.getItemLabel}
      getOptionKey={item => item.id}
      value={selectedItem}
      onChange={(e, v) => {
        setSelectedItem(v);
        props.setSelectedId(v?.id ?? null);
      }}
      isOptionEqualToValue={(option, value) => option.id === value.id}
      inputValue={inputValue}
      onInputChange={(e, v) => {
        setInputValue(v);
        searchDelayed();
      }}
      filterOptions={x => x}
      renderInput={(params) => (
        <TextField {...params} label={props.label} variant="outlined" required={props.required} />
      )}
    />
  );
}
