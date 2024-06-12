"use client";

import { usePathname, useSearchParams, useRouter } from "next/navigation";
import { useContext, useEffect } from "react";
import { Services, ServicesContext } from "./ServiceProvider";

const authPaths = ["/", "/auth/login", "/auth/register"];
const rootPath = "/";
const dashboardPath = "/dashboard";

export default function SecurityService({ 
  children 
}: {
  children: React.ReactNode
}) {
  const pathname = usePathname();
  const searchParams = useSearchParams();
  const router = useRouter();
  const { authService } = useContext<Services>(ServicesContext);
  
  const rerouteIfNecessary = async () => {
    let authed;

    try {
      await authService.getMe();
      authed = true;
    } catch (error) {
      authed = false;
    }

    if (authed && authPaths.includes(pathname))
      router.replace(dashboardPath);

    if (!authed && !authPaths.includes(pathname))
      router.replace(rootPath);
  } 
 
  useEffect(() => {
    rerouteIfNecessary();
  }, [pathname, searchParams]);

  return (children);
}