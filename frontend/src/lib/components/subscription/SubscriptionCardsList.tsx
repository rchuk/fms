"use client";

import { Box } from "@mui/material";
import SubscriptionCard from "./SubscriptionCard";
import { SubscriptionKind, UserSelfResponse } from "../../../../generated";
import { useContext, useEffect, useState } from "react";
import { ServicesContext } from "@/lib/services/ServiceProvider";
import { getRequestError } from "@/lib/utils/RequestUtils";
import { AlertContext } from "@/lib/services/AlertService";

export default function SubscriptionCardsList() {
  const [activeSubscriptionKind, setActiveSubscriptionKind] = useState<SubscriptionKind | null>(null);
  const { authService } = useContext(ServicesContext);
  const showAlert = useContext(AlertContext);

  useEffect(() => {
    authService.getMe()
    .then((response: UserSelfResponse) => {
      setActiveSubscriptionKind(response.subscriptionKind);
    })
    .catch((e) => {
      getRequestError(e).then(m => showAlert(m, "error"));
    });
  }, []);

  return (
    <Box
      sx={{
        height: "100%",
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
        "@media (max-width: 600px)": {
          height: "auto",
        }
      }}
    >
      <Box
        sx={{
          display: "flex",
          flexDirection: "row",
          justifyContent: "space-evenly",
          alignItems: "start",
          "@media (max-width: 600px)": {
            height: "auto",
            flexDirection: "column",
            alignItems: "center"
          }
        }}
      >
        <SubscriptionCard 
          header="Family" 
          price="9.99$" 
          subscriptionKind="FAMILY"
          activeSubscriptionKind={activeSubscriptionKind}
          setActiveSubscriptionKind={setActiveSubscriptionKind}
        >
          Want to try out the power of shared workspaces, but do not want to spend too much?
          Then this plan is just for you. With this plan you can create a shared workspace for 4
          users. It is a perfect option for families and small teams.
        </SubscriptionCard>
        <SubscriptionCard 
          header="Business" 
          price="29.99$" 
          subscriptionKind="BUSINESS"
          activeSubscriptionKind={activeSubscriptionKind}
          setActiveSubscriptionKind={setActiveSubscriptionKind}
        >
          You are running a business and 4 users in a shared workspace is not enough for you? Then
          try out this plan. Remove the limit of 4 users in a workspace and enjoy the flexibility
          of an unlimited shared workspace right now.
        </SubscriptionCard>
        <SubscriptionCard 
          header="Business Unlimited" 
          price="49.99$" 
          subscriptionKind="BUSINESS_UNLIMITED"
          activeSubscriptionKind={activeSubscriptionKind}
          setActiveSubscriptionKind={setActiveSubscriptionKind}
        >
          Single shared workspace is not enough for you? Then you definitely should buy this plan. 
          With it, you can have unlimited amout of shared workspaces and organizations.
        </SubscriptionCard>
      </Box>
    </Box>
  )
}