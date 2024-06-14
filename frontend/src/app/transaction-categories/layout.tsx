"use client";

import Nav from "@/lib/components/common/Nav"

export default function TransactionCategoriesLayout({
  children
}: {
  children: React.ReactNode
}) {
  return (
    <Nav>
      {children}
    </Nav>
  )
}