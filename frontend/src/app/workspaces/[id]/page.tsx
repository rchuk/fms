"use client";

import {Box} from "@mui/material";
import TransactionList from "@/lib/components/transaction/TransactionList";
import {WorkspaceResponse} from "../../../../generated";
import {useContext, useEffect, useState} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import {AlertContext} from "@/lib/services/AlertService";
import {getRequestError} from "@/lib/utils/RequestUtils";

export default function WorkspacePage({ params }: { params: { id: number } }) {
  const [workspace, setWorkspace] = useState<WorkspaceResponse | null>(null);
  const { workspaceService } = useContext(ServicesContext);
  const showAlert = useContext(AlertContext);

  function fetch() {
    const fetch = async() => {
      const response = await workspaceService.getWorkspace({ id: params.id });
      setWorkspace(response);
    };

    fetch().catch(e => getRequestError(e).then(m => showAlert(m, "error")))
  }

  useEffect(() => {
    fetch();
  }, []);

  const canCreateTransaction = workspace?.role !== undefined && workspace?.role !== "VIEWER";

  return (
    <Box display="flex" flexDirection="column" height="100%" padding={2} boxSizing="border-box">
      <TransactionList workspaceId={params.id} enableCreation={canCreateTransaction} />
    </Box>
  );
}
