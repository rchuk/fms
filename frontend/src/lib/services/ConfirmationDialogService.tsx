import * as React from 'react';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogTitle from '@mui/material/DialogTitle';
import {createContext, useState} from "react";
import {DialogContent, DialogContentText} from "@mui/material";

function confirmationDialogProviderMissing(props: ConfirmationDialogProps) {
  throw new Error("ConfirmationDialogProvider is missing");
}

export type ConfirmationDialogProps = {
  confirm: () => void,
  cancel?: () => void,
  title?: string,
  message?: string
};

export const ConfirmationDialogContext = createContext(confirmationDialogProviderMissing);

export default function ConfirmationDialogProvider({ children }: { children: React.ReactNode }) {
  const [isOpen, setIsOpen] = useState<boolean>(false);
  const [props, setProps] = useState<ConfirmationDialogProps | null>(null);

  function showConfirmation(props: ConfirmationDialogProps) {
    setIsOpen(true);
    setProps(props);
  }

  function close() {
    setIsOpen(false);
    setProps(null);
  }

  function handleConfirm() {
    close();
    props?.confirm?.();
  }

  function handleCancel() {
    close();
    props?.cancel?.();
  }

  return (
    <ConfirmationDialogContext.Provider value={showConfirmation}>
      {children}
      <Dialog
        open={isOpen}
        onClose={handleCancel}
      >
        <DialogTitle>
          {props?.title ?? "Ви впевнені?"}
        </DialogTitle>
        <DialogContent>
          <DialogContentText>
            {props?.message ?? ""}
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCancel}>Ні</Button>
          <Button onClick={handleConfirm} autoFocus>Так</Button>
        </DialogActions>
      </Dialog>
    </ConfirmationDialogContext.Provider>
  );
}
