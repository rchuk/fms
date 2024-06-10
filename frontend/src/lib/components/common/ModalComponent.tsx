import {ReactElement} from "react";
import {Dialog, DialogContent} from "@mui/material";

export function useOpenModal(setModalContent: (value: ReactElement | null) => void): (value: ReactElement) => void {
  return setModalContent;
}

export function useCloseModal(setModalContent: (value: ReactElement | null) => void): () => void {
  return () => setModalContent(null);
}

export function useModalControls(setModalContent: (value: ReactElement | null) => void): [(value: ReactElement) => void, () => void] {
  return [
    useOpenModal(setModalContent),
    useCloseModal(setModalContent)
  ];
}

export function useModalClosingCallback<RetT>(setModalContent: (value: ReactElement | null) => void, callback: () => RetT): () => RetT {
  return () => {
    const res = callback();
    setModalContent(null);

    return res;
  };
}

type ModalComponentProps = {
  content: ReactElement | null,
  setContent: (value: ReactElement | null) => void
}

export default function ModalComponent(props: ModalComponentProps) {
  return (
    <Dialog open={props.content !== null} onClose={() => props.setContent(null)}>
      <DialogContent sx={{ padding: 4}}>
        {props.content}
      </DialogContent>
    </Dialog>
  );
}
