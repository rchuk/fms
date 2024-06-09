"use client";

import {Link, Typography} from "@mui/material";
import CredentialsComponent from "@/lib/components/auth/CredentialsComponent";
import {useContext} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import {SessionServiceContext} from "@/lib/services/SessionService";
import {AlertContext} from "@/lib/services/AlertService";
import {getRequestError} from "@/lib/utils/RequestUtils";
import {useRouter} from "next/navigation";

export default function LoginPage() {
  const { authService } = useContext(ServicesContext);
  const sessionData = useContext(SessionServiceContext);
  const showAlert = useContext(AlertContext);
  const router = useRouter();

  function login(email: string, password: string) {
    const fetch = async() => {
      const result = await authService.register({ userRegisterRequest: { email, password } });

      sessionData.setAccessToken(result.accessToken);
    };

    fetch()
      .then(_ => router.push("/"))
      .catch(e => getRequestError(e).then(m => showAlert(m, "error")));
  }

  return (
    <main>
      <Typography variant="h3">
        Login
      </Typography>
      <CredentialsComponent handleSubmit={login}/>
      <div>
        Need an account? <Link href="/auth/register">Register here</Link>
      </div>
    </main>
  );
}
