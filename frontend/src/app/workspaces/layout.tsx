"use client";

import Nav from "@/lib/components/common/Nav"

export default function WorkspacesLayout({
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