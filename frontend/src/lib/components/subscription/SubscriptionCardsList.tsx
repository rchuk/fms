"use client";

import SubscriptionCard from "./SubscriptionCard";

export default function SubscriptionCardsList() {
  return (
    <>
      <SubscriptionCard header="Family plan" price="9.99$" subscriptionKind="FAMILY">
        Buy family plan
      </SubscriptionCard>
      <SubscriptionCard header="Business plan" price="29.99$" subscriptionKind="BUSINESS">
        Buy business plan
      </SubscriptionCard>
      <SubscriptionCard header="Business unlimited plan" price="49.99$" subscriptionKind="BUSINESS_UNLIMITED">
        Buy business unlimited plan
      </SubscriptionCard>
    </>
  )
}