"use client";

import {Link, Typography} from "@mui/material";
import CredentialsComponent from "@/lib/components/auth/CredentialsComponent";
import {useContext} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import {getRequestError} from "@/lib/utils/RequestUtils";
import {AlertContext} from "@/lib/services/AlertService";
import {SessionServiceContext} from "@/lib/services/SessionService";
import {useRouter} from "next/navigation";

export default function RegisterPage() {
  const { authService } = useContext(ServicesContext);
  const sessionData = useContext(SessionServiceContext);
  const showAlert = useContext(AlertContext);
  const router = useRouter();

  function register(email: string, password: string) {
    const fetch = async() => {
      const result = await authService.register({ userRegisterRequest: { email, password } });

      sessionData.setAccessToken(result.accessToken);
    };

    fetch()
      .then(_ => {
        showAlert("Реєстрація пройшла успішно", "success");
        router.push("/");
      })
      .catch(e => getRequestError(e).then(m => showAlert(m, "error")));
  }

  return (
    <>
      <Typography variant="h3">
        Register
      </Typography>
      <CredentialsComponent handleSubmit={register}/>
      <div>
        Already have an account? <Link href="/auth/login">Login here</Link>
      </div>
    </>
  );
}
