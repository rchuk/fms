import {ReactElement, useContext, useEffect, useState} from "react";
import {Box, Pagination} from "@mui/material";
import {getRequestError} from "@/lib/utils/RequestUtils";
import {AlertContext} from "@/lib/services/AlertService";


type PaginatedListProps<ItemT> = {
  fetch: (offset: number, limit: number) => Promise<[number, ItemT[]]>,
  pageSize: number,
  renderItem: (data: ItemT) => ReactElement,

  itemsData?: ItemT[],
  setItemsData?: (value: ItemT[]) => void,

  items?: ReactElement[],
  setItems?: (value: ReactElement[]) => void
}

export default function PaginatedList<ItemT>(props: PaginatedListProps<ItemT>) {
  const [totalItemCount, setTotalItemCount] = useState<number>(0);
  const [pageCount, setPageCount] = useState<number>(1);
  const [pageIndex, setPageIndex] = useState<number>(0);
  const [internalItemsData, setInternalItemsData] = useState<ItemT[]>([]);
  const [internalItems, setInternalItems] = useState<ReactElement[]>([]);
  const showAlert = useContext(AlertContext);

  const [itemsData, setItemsData] = [props.itemsData ?? internalItemsData, props.setItemsData ?? setInternalItemsData];
  const [items, setItems] = [props.items ?? internalItems, props.setItems ?? setInternalItems];

  useEffect(() => {
    setPageCount(Math.ceil(totalItemCount / props.pageSize));
  }, [totalItemCount]);

  useEffect(() => {
    const fetch = async() => {
      const [newTotalItemCount, newItemsData] = await props.fetch(pageIndex * props.pageSize, props.pageSize);

      setTotalItemCount(newTotalItemCount);
      setItemsData(newItemsData);
    };

    fetch().catch(e => getRequestError(e).then(m => showAlert(m, "error")));
  }, [pageIndex]);

  useEffect(() => {
    setItems(itemsData.map(props.renderItem));
  }, [itemsData]);

  return (
    <Box display="flex" flexDirection="column" flex={1}>
      <Box display="flex" flexDirection="column" flex={1} rowGap={2} padding={2}>
        {...items}
      </Box>
      <Pagination
        count={pageCount}
        variant="outlined"
        color="primary"
        onChange={(e, page) => setPageIndex(page - 1)}
        sx={{
          alignSelf: "center"
        }}
      />
    </Box>
  );
}
