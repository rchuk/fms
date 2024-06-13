import {IconButton} from "@mui/material";
import {SortDirection} from "../../../../generated";
import ArrowUpwardIcon from '@mui/icons-material/ArrowUpward';
import ArrowDownwardIcon from '@mui/icons-material/ArrowDownward';


interface SortCriteria {
  sortDirection?: SortDirection
}

type SortDirectionSelectProps<CriteriaT extends SortCriteria> = {
  criteria: CriteriaT,
  setCriteria: (value: CriteriaT) => void,

  searchDelayed: () => void
}

export default function SortDirectionSelect<CriteriaT extends SortCriteria>(props: SortDirectionSelectProps<CriteriaT>) {
  const defaultSortDirection = SortDirection.Ascending;

  function toggle() {
    const newSortDirection = (props.criteria.sortDirection ?? defaultSortDirection) === SortDirection.Ascending
      ? SortDirection.Descending
      : SortDirection.Ascending;

    props.setCriteria({...props.criteria, sortDirection: newSortDirection});
    props.searchDelayed();
  }

  return (
    <IconButton onClick={toggle}>
      {
        (props.criteria.sortDirection ?? defaultSortDirection) == SortDirection.Ascending
          ? <ArrowUpwardIcon />
          : <ArrowDownwardIcon />
      }
    </IconButton>
  );
}
