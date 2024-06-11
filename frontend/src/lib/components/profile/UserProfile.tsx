"use client";

import { Typography } from "@mui/material";
import { SubscriptionKind } from "../../../../generated";

export type UserProfileProps = {
  firstName: string,
  lastName: string,
  email: string,
  subscriptionKind?: SubscriptionKind
}

export default function UserProfile({
  firstName = "",
  lastName = "",
  email = "",
  subscriptionKind = undefined
}: UserProfileProps) {
  return (
    <>
      <Typography variant="h3">Full name: {firstName} {lastName}</Typography>
      <Typography variant="h4">Email: {email}</Typography>
      <Typography variant="h4">Subscription: {subscriptionKind ? subscriptionKind : "NONE"}</Typography>
    </>
  )
}