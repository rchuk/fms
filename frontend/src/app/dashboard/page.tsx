"use client";

import { ServicesContext } from "@/lib/services/ServiceProvider";
import { useRouter } from "next/navigation";
import { useContext, useEffect } from "react";

export default function DashboardPage() {
  const { authService } = useContext(ServicesContext);
  const router = useRouter();

  useEffect(() => {
    authService.getMe()
    .then((response) => {
      router.replace("/workspaces/" + response.privateWorkspace.id);
    })
    .catch(() => {
      router.back();
    })
  }, []);

  return null;
}
