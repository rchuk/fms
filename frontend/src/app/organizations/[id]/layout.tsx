"use client";

import PageTabs, {NavigationTabKind} from "@/lib/components/common/PageTabs";
import {useRouter} from "next/navigation";

export default function OrganizationLayout({ children, params }: {
  children: React.ReactNode,
  params: { id: number }
}) {
  const router = useRouter();

  function navigate(tab: NavigationTabKind) {
    switch (tab) {
      case "Main":
        router.push(`/organizations/${params.id}`);
        return;
      case "TransactionCategories":
        router.push(`/organizations/${params.id}/transaction-categories`);
        return;
      case "Users":
        router.push(`/organizations/${params.id}/users`);
        return;
    }
  }

  return (
    <>
      <PageTabs navigate={navigate} mainLabel="Робочі простори" />
      {children}
    </>
  )
}
