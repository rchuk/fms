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
import TransactionPlot from "./TransactionPlot";

type TransactionListProps = {
  workspaceId: number
}

export default function TransactionList(props: TransactionListProps) {
  const [criteria, setCriteria] = useState<ListTransactionsRequest>({
    workspaceId: props.workspaceId
  });
  const [itemsData, setItemsData] = useState<TransactionResponse[]>([]);
  const [modalContent, setModalContent] = useState<ReactElement | null>(null);
  const [isPlotDirty, setIsPlotDirty] = useState<boolean>(false);
  const [isListDirty, setIsListDirty] = useState<boolean>(false);
  const { transactionService } = useContext(ServicesContext);

  function setDirty() {
    setIsListDirty(true);
    setIsPlotDirty(true);
  }

  const [openModal, closeModal] = useModalControls(setModalContent);
  const onSave = useModalClosingCallback(setModalContent, setDirty);

  function create() {
    openModal(<TransactionUpsert initialId={null} workspaceId={props.workspaceId} onError={closeModal} cancel={closeModal} onSave={onSave} />);
  }

  function renderCard(data: TransactionResponse) {
    return <TransactionListCard item={data} />
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
        criteria.startDate != null && criteria.categoryKind != null
          ? <TransactionPlot criteria={criteria} isDirty={isPlotDirty} setIsDirty={setIsPlotDirty} />
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
      <FloatingAddButton onClick={create} />
      <ModalComponent content={modalContent} setContent={setModalContent} />
    </>
  );
}
