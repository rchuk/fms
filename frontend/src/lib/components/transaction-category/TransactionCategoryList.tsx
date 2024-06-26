"use client";

import PaginatedList from "@/lib/components/common/PaginatedList";
import {TransactionCategoryResponse} from "../../../../generated";
import TransactionCategoryListCard from "@/lib/components/transaction-category/TransactionCategoryListCard";
import {ReactElement, useContext, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import FloatingAddButton from "../common/FloatingAddButton";
import ModalComponent, {useModalClosingCallback, useModalControls} from "../common/ModalComponent";
import {TransactionCategorySource} from "@/lib/components/transaction-category/Common";
import TransactionCategoryUpsert from "./TransactionCategoryUpsert";

type TransactionCategoryListProps = {
  source: TransactionCategorySource,

  enableCreation?: boolean
}

export default function TransactionCategoryList(props: TransactionCategoryListProps) {
  const [modalContent, setModalContent] = useState<ReactElement | null>(null);
  const [isDirty, setIsDirty] = useState<boolean>(false);
  const { transactionCategoryService } = useContext(ServicesContext);

  const [openModal, closeModal] = useModalControls(setModalContent);
  const onSave = useModalClosingCallback(setModalContent, () => setIsDirty(true));

  function create() {
    return openModal(<TransactionCategoryUpsert initialId={null} source={props.source} onError={closeModal} cancel={closeModal} onSave={onSave} isLocked={false} />);
  }

  function update(id: number) {
    return openModal(<TransactionCategoryUpsert initialId={id} source={props.source} onError={closeModal} cancel={closeModal} onSave={onSave} isLocked={!props.enableCreation} />);
  }

  function renderCard(data: TransactionCategoryResponse) {
    return <TransactionCategoryListCard item={data} onClick={update}/>
  }

  async function fetch(offset: number, limit: number) {
    switch (props.source.kind) {
      case "user":
        return await transactionCategoryService.listUserTransactionCategories({ offset, limit });
      case "organization":
        return await transactionCategoryService.listOrganizationTransactionCategories({
          organizationId: props.source.organizationId,
          offset,
          limit
        });
      case "workspace":
        return await transactionCategoryService.listWorkspaceTransactionCategories({
          workspaceId: props.source.workspaceId,
          offset,
          limit
        });
    }
  }

  return (
    <>
      <PaginatedList fetch={fetch} pageSize={10} renderItem={renderCard} isDirty={isDirty} setIsDirty={setIsDirty}/>
      { props.enableCreation && <FloatingAddButton onClick={create}/> }
      <ModalComponent content={modalContent} setContent={setModalContent}/>
    </>
  );
}
