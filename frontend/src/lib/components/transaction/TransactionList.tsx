"use client";

import PaginatedList from "@/lib/components/common/PaginatedList";
import {TransactionResponse} from "../../../../generated";
import TransactionListCard from "@/lib/components/transaction/TransactionListCard";
import {ReactElement, useContext, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import FloatingAddButton from "../common/FloatingAddButton";
import ModalComponent, {useModalClosingCallback, useModalControls} from "@/lib/components/common/ModalComponent";
import TransactionUpsert from "@/lib/components/transaction/TransactionUpsert";

type TransactionListProps = {
  workspaceId: number
}

export default function TransactionList(props: TransactionListProps) {
  const [modalContent, setModalContent] = useState<ReactElement | null>(null);
  const [isDirty, setIsDirty] = useState<boolean>(false);
  const { transactionService } = useContext(ServicesContext);

  const [openModal, closeModal] = useModalControls(setModalContent);
  const onSave = useModalClosingCallback(setModalContent, () => setIsDirty(true));

  function create() {
    openModal(<TransactionUpsert initialId={null} workspaceId={props.workspaceId} onError={closeModal} cancel={closeModal} onSave={onSave} />);
  }

  function renderCard(data: TransactionResponse) {
    return <TransactionListCard item={data} />
  }

  async function fetch(offset: number, limit: number) {
    // TODO: Add criteria
    return await transactionService.listTransactions({ workspaceId: props.workspaceId , offset, limit });
  }

  return (
    <>
      <PaginatedList fetch={fetch} pageSize={10} renderItem={renderCard} isDirty={isDirty} setIsDirty={setIsDirty}/>
      <FloatingAddButton onClick={create} />
      <ModalComponent content={modalContent} setContent={setModalContent} />
    </>
  );
}
