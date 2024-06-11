"use client";

import SubscriptionCardsList from "@/lib/components/subscription/SubscriptionCardsList";
import { ServicesContext } from "@/lib/services/ServiceProvider";
import { useContext } from "react";

export default function SubscriptionsPage() {
  const { subscriptionService } = useContext(ServicesContext);
  return (
    <SubscriptionCardsList />
  );
}