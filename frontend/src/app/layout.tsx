"use client"

import React, {useState} from "react";
import ServicesProvider, {createServices, Services} from "@/lib/services/ServiceProvider";
import {AdapterDayjs} from "@mui/x-date-pickers/AdapterDayjs";
import {createTheme, ThemeProvider} from "@mui/material";
import {LocalizationProvider} from "@mui/x-date-pickers";
import {Roboto} from "next/font/google";
import { AlertProvider } from "@/lib/services/AlertService";

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

  return (
    <html lang="uk" className={roboto.className} style={{ minHeight: "100vh", height: "100%" }}>
      <body style={{ margin: 0, height: "100%" }}>
        <LocalizationProvider
          dateAdapter={AdapterDayjs} adapterLocale="uk"
        >
          <AlertProvider>
            <ServicesProvider services={services}>
              <ThemeProvider theme={theme}>
                {children}
              </ThemeProvider>
            </ServicesProvider>
          </AlertProvider>
        </LocalizationProvider>
      </body>
    </html>
  )
}
