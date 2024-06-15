"use client";

import PageTabs, {NavigationTabKind} from "@/lib/components/common/PageTabs";
import {useRouter} from "next/navigation";

export default function WorkspaceLayout({ children, params }: {
  children: React.ReactNode,
  params: { id: number }
}) {
  const router = useRouter();

  function navigate(tab: NavigationTabKind) {
    switch (tab) {
      case "Transactions":
        router.push(`/workspaces/${params.id}`);
        return;
      case "TransactionCategories":
        router.push(`/workspaces/${params.id}/transaction-categories`);
        return;
      case "Users":
        router.push(`/workspaces/${params.id}/users`);
        return;
    }
  }

  return (
    <>
      <PageTabs navigate={navigate} />
      {children}
    </>
  )
}
