"use client";

import UserProfile from "@/lib/components/profile/UserProfile";
import { AlertContext } from "@/lib/services/AlertService";
import { ServicesContext } from "@/lib/services/ServiceProvider";
import { getRequestError } from "@/lib/utils/RequestUtils";
import { useContext, useEffect, useState } from "react";
import { UserSelfResponse } from "../../../generated";
import { Link, Typography } from "@mui/material";
import { redirect } from "next/navigation";
import { useRouter } from "next/router";

export default function ProfilePage() {
  const { authService } = useContext(ServicesContext);
  const showAlert = useContext(AlertContext)
  const [user, setUser] = useState<UserSelfResponse | null>(null);
  useEffect(() => {
    authService.getMe()
    .then((response: UserSelfResponse) => {
      setUser(response);
    })
    .catch((e) => {
      getRequestError(e).then(m => showAlert(m, "error"));
    });
  }, []);
  
  if (user === null) {
    return (
      <Typography variant="body1">You are not logged in. <Link href="/auth/login">Log in here</Link></Typography>
    );
  }

  return (
    <>
      <UserProfile {...user} />
      <Typography variant="body1">Want more features? <Link href="/subscriptions">Upgrade subscription tier</Link></Typography>
    </>
  );
}