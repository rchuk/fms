"use client";

import { Button, Typography } from "@mui/material";
import { SubscriptionKind } from "../../../../generated";
import { useContext } from "react";
import { ServicesContext } from "@/lib/services/ServiceProvider";
import { AlertContext } from "@/lib/services/AlertService";
import { getRequestError } from "@/lib/utils/RequestUtils";

type SubscriptionCardProps = {
  children: React.ReactNode,
  header: string,
  price: string,
  subscriptionKind: SubscriptionKind
}

export default function SubscriptionCard({
  children = "",
  header = "",
  price = "",
  subscriptionKind
}: SubscriptionCardProps) {
  const { subscriptionService } = useContext(ServicesContext);
  const showAlert = useContext(AlertContext);
  function buySubscription() {
    subscriptionService
    .buySubscription( { buySubscriptionRequest: { kind: subscriptionKind } })
    .then(() => {
      showAlert(`Ви успішно купили підписку рівня ${subscriptionKind}`, "success");
    })
    .catch((e) => {
      getRequestError(e).then((m) => showAlert(m, "error"));
    });
  }

  return (
    <>
      <Typography variant="h2">{ header }</Typography>
      {children}
      <>
        <Button onClick={buySubscription}>
          Buy for {price}
        </Button>
      </>
    </>
  );
}