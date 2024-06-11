"use client";

import UserProfile from "@/lib/components/profile/UserProfile";
import { AlertContext } from "@/lib/services/AlertService";
import { ServicesContext } from "@/lib/services/ServiceProvider";
import { getRequestError } from "@/lib/utils/RequestUtils";
import { useContext, useEffect, useState } from "react";
import { UserSelfResponse } from "../../../generated";

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
  
  return (
    <UserProfile {...(user ? user : {})} />
  )
}