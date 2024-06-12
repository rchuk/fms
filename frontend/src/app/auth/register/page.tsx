"use client";

import {Box} from "@mui/material";
import {useContext} from "react";
import {ServicesContext} from "@/lib/services/ServiceProvider";
import {getRequestError} from "@/lib/utils/RequestUtils";
import {AlertContext} from "@/lib/services/AlertService";
import {SessionServiceContext} from "@/lib/services/SessionService";
import {useRouter} from "next/navigation";
import AuthCard from "@/lib/components/auth/AuthCard";
import RegisterForm from "@/lib/components/auth/RegisterForm";

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
        router.push("/dashboard");
      })
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
        header={"Register"} 
        otherOptionText={"Already have an account?"} 
        otherOptionLinkText={"Login here"} 
        otherOptionLink={"/auth/login"}
      >
        <RegisterForm handleSubmit={register} />
      </AuthCard>
    </Box>
  );
}
