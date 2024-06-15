"use client";

import PaginatedList from "@/lib/components/common/PaginatedList";
import {ListTransactionsRequest, TransactionResponse} from "../../../../generated";
import TransactionListCard from "@/lib/components/transaction/TransactionListCard";
import {ReactElement, useContext, useEffect, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import FloatingAddButton from "../common/FloatingAddButton";
import ModalComponent, {useModalClosingCallback, useModalControls} from "@/lib/components/common/ModalComponent";
import TransactionUpsert from "@/lib/components/transaction/TransactionUpsert";
import TransactionFilter from "./TransactionFilter";
import TransactionPlotPie from "./plot/TransactionPlotPie";
import TransactionPlotStack from "@/lib/components/transaction/plot/TransactionPlotStack";
import {TransactionPlotKind} from "@/lib/components/transaction/plot/Common";

type TransactionListProps = {
  workspaceId: number,

  enableCreation?: boolean
}

export default function TransactionList(props: TransactionListProps) {
  const [criteria, setCriteria] = useState<ListTransactionsRequest>({
    workspaceId: props.workspaceId
  });
  const [itemsData, setItemsData] = useState<TransactionResponse[]>([]);
  const [modalContent, setModalContent] = useState<ReactElement | null>(null);
  const [plotKind, setPlotKind] = useState<TransactionPlotKind>("category");
  const [isPlotDirty, setIsPlotDirty] = useState<boolean>(false);
  const [isListDirty, setIsListDirty] = useState<boolean>(false);
  const { transactionService } = useContext(ServicesContext);

  useEffect(() => {
    setIsPlotDirty(true);
  }, [plotKind]);

  function setDirty() {
    setIsListDirty(true);
    setIsPlotDirty(true);
  }

  const [openModal, closeModal] = useModalControls(setModalContent);
  const onSave = useModalClosingCallback(setModalContent, setDirty);

  const showPiePlot = criteria.startDate != null && criteria.categoryKind != null;
  const showStackPlot = criteria.startDate != null && criteria.categoryKind == null;

  function create() {
    openModal(<TransactionUpsert initialId={null} workspaceId={props.workspaceId} onError={closeModal} cancel={closeModal} onSave={onSave} isLocked={false}/>);
  }
  
  function update(id: number) {
    openModal(<TransactionUpsert initialId={id} workspaceId={props.workspaceId} onError={closeModal} cancel={closeModal} onSave={onSave} isLocked={!props.enableCreation} />);
  }

  function renderCard(data: TransactionResponse) {
    return <TransactionListCard item={data} onClick={update} />
  }

  async function fetch(offset: number, limit: number) {
    return await transactionService.listTransactions({
      ...criteria,
      offset,
      limit
    });
  }

  const head = (
    <>
      <TransactionFilter criteria={criteria} setCriteria={setCriteria} onChange={setDirty} />
      {
        showPiePlot
          ? <TransactionPlotPie kind={plotKind} setKind={setPlotKind} criteria={criteria} isDirty={isPlotDirty} setIsDirty={setIsPlotDirty} />
          : null
      }
      {
        showStackPlot
          ? <TransactionPlotStack kind={plotKind} setKind={setPlotKind} criteria={criteria} isDirty={isPlotDirty} setIsDirty={setIsPlotDirty} />
          : null
      }
    </>
  );

  return (
    <>
      <PaginatedList
        fetch={fetch}
        pageSize={10}
        renderItem={renderCard}
        isDirty={isListDirty}
        setIsDirty={setIsListDirty}
        itemsData={itemsData}
        setItemsData={setItemsData}
        head={head}
      />
      { props.enableCreation && <FloatingAddButton onClick={create} /> }
      <ModalComponent content={modalContent} setContent={setModalContent} />
    </>
  );
}
