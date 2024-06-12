"use client";

import React, {AnchorHTMLAttributes, forwardRef, useEffect, useState} from "react";
import ServicesProvider, {createServices, Services} from "@/lib/services/ServiceProvider";
import {AdapterDayjs} from "@mui/x-date-pickers/AdapterDayjs";
import {createTheme, CssBaseline, ThemeProvider} from "@mui/material";
import {LocalizationProvider} from "@mui/x-date-pickers";
import {Roboto} from "next/font/google";
import { AlertProvider } from "@/lib/services/AlertService";
import {Configuration} from "../../generated";
import NextLink, {LinkProps} from 'next/link';
import SessionServiceProvider, {getCachedAccessToken} from "@/lib/services/SessionService";

const roboto = Roboto({
  weight: "400",
  subsets: ["latin", "cyrillic"]
});

type LinkBehaviourProps = LinkProps & Omit<AnchorHTMLAttributes<HTMLAnchorElement>, 'href'>;

const LinkBehaviour = forwardRef<HTMLAnchorElement, LinkBehaviourProps>(function LinkBehaviour(props, ref) {
  return <NextLink ref={ref} {...props} />;
});

const theme = createTheme({
  palette: {
    mode: "dark",
    primary: {
      main: "#388e3c"
    },
    secondary: {
      main: "#388a8e"
    }
  },
  components: {
    MuiLink: {
      defaultProps: {
        component: LinkBehaviour
      }
    },
    MuiButtonBase: {
      defaultProps: {
        LinkComponent: LinkBehaviour
      }
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
  const [isReady, setIsReady] = useState<boolean>(false);

  useEffect(() => {
    const token = accessToken ?? getCachedAccessToken();

    const config = token !== null
      ? new Configuration({
          headers: {
            "Authorization": "Bearer " + token
          }
        })
      : null;

    setAccessToken(token);
    setServices(createServices(config));
    setIsReady(true);
  }, [accessToken]);

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline enableColorScheme />
      <html lang="uk" className={roboto.className} style={{ minHeight: "100vh", height: "100%" }}>
        <body style={{ margin: 0, height: "100%" }}>
          <LocalizationProvider
            dateAdapter={AdapterDayjs} adapterLocale="uk"
          >
            <AlertProvider>
              <ServicesProvider services={services}>
                <SessionServiceProvider accessToken={accessToken} setAccessToken={setAccessToken}>
                  {isReady ? children : []}
                </SessionServiceProvider>
              </ServicesProvider>
            </AlertProvider>
          </LocalizationProvider>
        </body>
      </html>
    </ThemeProvider>
  )
}
