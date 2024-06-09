"use client"

import React, {ReactElement, ReactNode, useState} from "react";
import {AppProps} from "next/app";
import {Roboto} from "next/font/google";
import {NextPage} from "next";
import {createTheme, ThemeProvider} from "@mui/material";
import {useRouter} from "next/router";
import {LocalizationProvider} from "@mui/x-date-pickers";
import {AdapterDayjs} from "@mui/x-date-pickers/AdapterDayjs";
import BasicLayout from "@/app/layout/BasicLayout";
import "./globals.css";
import ServicesProvider, {createServices, Services} from "@/app/services/ServiceProvider";

export type NextPageWithLayout<P = {}, IP = P> = NextPage<P, IP> & {
  getLayout?: (page: ReactElement) => ReactNode
}

type AppPropsWithLayout = AppProps & {
  Component: NextPageWithLayout
}

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

export default function MyApp({ Component, pageProps }: AppPropsWithLayout) {
  const getLayout = Component.getLayout ?? ((page) => page);

  const [services, setServices] = useState<Services>(createServices);
  const router = useRouter();

  return (
    <main className={roboto.className}>
      <LocalizationProvider
        dateAdapter={AdapterDayjs} adapterLocale="uk"
      >
        <ServicesProvider services={services}>
          <ThemeProvider theme={theme}>
            <BasicLayout>
              {getLayout(<Component {...pageProps} />)}
            </BasicLayout>
          </ThemeProvider>
        </ServicesProvider>
      </LocalizationProvider>
    </main>
  );
}
