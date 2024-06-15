"use client";

import React, {AnchorHTMLAttributes, Suspense, forwardRef, useEffect, useState} from "react";
import ServicesProvider, {createServices, Services} from "@/lib/services/ServiceProvider";
import {AdapterDayjs} from "@mui/x-date-pickers/AdapterDayjs";
import {createTheme, CssBaseline, responsiveFontSizes, ThemeProvider} from "@mui/material";
import {LocalizationProvider} from "@mui/x-date-pickers";
import {Roboto, Montserrat} from "next/font/google";
import { AlertProvider } from "@/lib/services/AlertService";
import {Configuration} from "../../generated";
import NextLink, {LinkProps} from 'next/link';
import SessionServiceProvider, {getCachedAccessToken} from "@/lib/services/SessionService";
import SecurityService from "@/lib/services/SecurityService";
import ConfirmationDialogProvider from "@/lib/services/ConfirmationDialogService";

const roboto = Roboto({
  weight: "400",
  subsets: ["latin", "cyrillic"]
});

const montserrat = Montserrat({
  weight: ["500", "600", "700"],
  subsets: ["latin", "cyrillic"],
  fallback: ["Arial", "sans-serif"]
});

type LinkBehaviourProps = LinkProps & Omit<AnchorHTMLAttributes<HTMLAnchorElement>, 'href'>;

const LinkBehaviour = forwardRef<HTMLAnchorElement, LinkBehaviourProps>(function LinkBehaviour(props, ref) {
  return <NextLink ref={ref} {...props} />;
});

let theme = createTheme({
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
  },
  typography: {
    h1: {
      fontFamily: montserrat.style.fontFamily
    },
    h2: {
      fontFamily: montserrat.style.fontFamily
    },
    h3: {
      fontFamily: montserrat.style.fontFamily
    },
    h4: {
      fontFamily: montserrat.style.fontFamily
    },
    h5: {
      fontFamily: montserrat.style.fontFamily
    }
  }
});

theme = responsiveFontSizes(theme);

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
              <ConfirmationDialogProvider>
                <ServicesProvider services={services}>
                  <SessionServiceProvider accessToken={accessToken} setAccessToken={setAccessToken}>
                    <Suspense fallback={null}>
                      {
                        isReady ? (
                          <SecurityService>
                            {children}
                          </SecurityService>
                        ) : []
                      }
                    </Suspense>
                  </SessionServiceProvider>
                </ServicesProvider>
              </ConfirmationDialogProvider>
            </AlertProvider>
          </LocalizationProvider>
        </body>
      </html>
    </ThemeProvider>
  )
}
