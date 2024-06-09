"use client";

import React, {useEffect, useState} from "react";
import ServicesProvider, {createServices, Services} from "@/lib/services/ServiceProvider";
import {AdapterDayjs} from "@mui/x-date-pickers/AdapterDayjs";
import {createTheme, ThemeProvider} from "@mui/material";
import {LocalizationProvider} from "@mui/x-date-pickers";
import {Roboto} from "next/font/google";
import { AlertProvider } from "@/lib/services/AlertService";
import {Configuration} from "../../generated";
import SessionServiceProvider, {getCachedAccessToken} from "@/lib/services/SessionService";

const roboto = Roboto({
  weight: "400",
  subsets: ["latin", "cyrillic"]
})

declare module '@mui/material/styles' {
  interface Palette {
    almostWhite: Palette["primary"];
  }

  interface PaletteOptions {
    almostWhite?: PaletteOptions["primary"];
  }
}

const theme = createTheme({
  palette: {
    primary: {
      main: "#8a0709"
    },
    secondary: {
      main: "#078a88"
    },
    almostWhite: {
      main: "#eeeeee"
    }
  }
});

export default function RootLayout({
  children,
}: {
  children: React.ReactNode
}) {
  const [services, setServices] = useState<Services>(createServices);
  const [accessToken, setAccessToken] = useState<string | null>(null);

  useEffect(() => {
    setAccessToken(getCachedAccessToken);
  }, []);

  useEffect(() => {
    let config = accessToken !== null
      ? new Configuration({
          headers: {
            "Authorization": "Bearer " + accessToken
          }
        })
      : null;

    setServices(createServices(config));
  }, [accessToken]);

  return (
    <html lang="uk" className={roboto.className} style={{ minHeight: "100vh", height: "100%" }}>
      <body style={{ margin: 0, height: "100%" }}>
        <ThemeProvider theme={theme}>
          <LocalizationProvider
            dateAdapter={AdapterDayjs} adapterLocale="uk"
          >
            <AlertProvider>
              <ServicesProvider services={services}>
                <SessionServiceProvider accessToken={accessToken} setAccessToken={setAccessToken}>
                  {children}
                </SessionServiceProvider>
              </ServicesProvider>
            </AlertProvider>
          </LocalizationProvider>
        </ThemeProvider>
      </body>
    </html>
  )
}
