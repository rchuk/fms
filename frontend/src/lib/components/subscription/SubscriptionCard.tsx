"use client";

import { Box, Button, Card, Typography } from "@mui/material";
import { SubscriptionKind } from "../../../../generated";
import { useContext } from "react";
import { ServicesContext } from "@/lib/services/ServiceProvider";
import { AlertContext } from "@/lib/services/AlertService";
import { getRequestError } from "@/lib/utils/RequestUtils";

type SubscriptionCardProps = {
  children: React.ReactNode,
  header: string,
  price: string,
  subscriptionKind: SubscriptionKind,
  activeSubscriptionKind: SubscriptionKind | null,
  setActiveSubscriptionKind: any
}

export default function SubscriptionCard({
  children = "",
  header = "",
  price = "",
  subscriptionKind,
  activeSubscriptionKind,
  setActiveSubscriptionKind
}: SubscriptionCardProps) {
  const { subscriptionService } = useContext(ServicesContext);
  const showAlert = useContext(AlertContext);
  const subscriptionToThis = activeSubscriptionKind == subscriptionKind;

  function buySubscription() {
    subscriptionService
    .buySubscription( { buySubscriptionRequest: { kind: subscriptionKind } })
    .then(() => {
      showAlert(`Ви успішно купили підписку рівня ${subscriptionKind}`, "success");
      setActiveSubscriptionKind(subscriptionKind);
    })
    .catch((e) => {
      getRequestError(e).then((m) => showAlert(m, "error"));
    });
  }

  return (
    <Card
      raised={subscriptionToThis}
      sx={{
        p: 4,
        borderRadius: "2rem",
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        width: "30%",
        "@media (max-width: 600px)": {
          mt: 4,
          width: "80%"
        }
      }}
    >
      <Typography 
        variant="h2"
        sx={{
          textAlign: "center"
        }}
      >
        { header }
      </Typography>
      <Typography 
        variant="h6"
        sx={{
          mt: 4,
          textAlign: "center"
        }}
      >
        {children}
      </Typography>
      <Box>
        <Button 
          disabled={subscriptionToThis}
          sx={{
            mt: 4,
            py: 2,
            width: "100%"
          }}
          onClick={buySubscription}
        >
          <Typography 
            variant="h6"
          >
            { 
              subscriptionToThis ? 
              (
                <>Current plan</>
              ) : 
              (
                <>Buy for {price}</>
              ) 
            }
          </Typography>
        </Button>
      </Box>
    </Card>
  );
}