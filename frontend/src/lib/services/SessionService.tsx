"use client"

import {createContext, PropsWithChildren} from "react";

export type SessionData = {
  accessToken: string | null,
  setAccessToken: (value: string | null) => void
}

export const SessionServiceContext = createContext<SessionData>(null!);

type SessionServiceProviderProps = {
  accessToken: string | null,
  setAccessToken: (value: string | null) => void
};

const accessTokenKey: string = "access-token";

export function getCachedAccessToken(): string | null {
  return localStorage.getItem(accessTokenKey)
}

export default function SessionServiceProvider(props: PropsWithChildren<SessionServiceProviderProps>) {
  function setToken(value: string | null) {
    if (value !== null)
      localStorage.setItem(accessTokenKey, value);
    else
      localStorage.removeItem(accessTokenKey);

    props.setAccessToken(value);
  }

  const sessionData: SessionData = {
    accessToken: props.accessToken,
    setAccessToken: setToken
  };

  return (
    <SessionServiceContext.Provider value={sessionData}>
      {props.children}
    </SessionServiceContext.Provider>
  )
}
