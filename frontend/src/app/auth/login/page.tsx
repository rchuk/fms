"use client";

import {Box} from "@mui/material";
import {useContext} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import {SessionServiceContext} from "@/lib/services/SessionService";
import {AlertContext} from "@/lib/services/AlertService";
import {getRequestError} from "@/lib/utils/RequestUtils";
import {useRouter} from "next/navigation";
import AuthCard from "@/lib/components/auth/AuthCard";
import LoginForm from "@/lib/components/auth/LoginForm";

export default function LoginPage() {
  const { authService } = useContext(ServicesContext);
  const sessionData = useContext(SessionServiceContext);
  const showAlert = useContext(AlertContext);
  const router = useRouter();

  function login(email: string, password: string) {
    const fetch = async() => {
      const result = await authService.login({ email, password });

      sessionData.setAccessToken(result.accessToken);
    };

    fetch()
      .then(_ => router.push("/dashboard"))
      .catch(e => getRequestError(e).then(m => showAlert(m, "error")));
  }

  return (
    <Box
      overflow="hidden" 
      height={"100vh"} 
      display={"flex"}
      flexDirection={"column"} 
      justifyContent={"center"}
      alignItems={"center"}
    >
      <AuthCard 
        header={"Login"} 
        otherOptionText={"Need an account?"} 
        otherOptionLinkText={"Register here"} 
        otherOptionLink={"/auth/register"}
      >
        <LoginForm handleSubmit={login} />
      </AuthCard>
    </Box>
  );
}
