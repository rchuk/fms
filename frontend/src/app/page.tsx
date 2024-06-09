"use client";

import { Button } from "@mui/material";
import styles from "./page.module.css";
import { useRouter } from "next/navigation";

export default function Home() {
  const router = useRouter();
  
  return (
    <main className={styles.main}>
      <div>
        Welcome to FMS
      </div>
      <Button onClick={() => router.push("/auth/login")}>
        Login
      </Button>
      <Button onClick={() => router.push("/auth/register")}>
        Register
      </Button>
    </main>
  );
}
