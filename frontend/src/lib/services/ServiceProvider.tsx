"use client"

import {
  Configuration,
  AuthApi,
  OrganizationApi,
  SubscriptionApi,
  TransactionApi,
  TransactionCategoryApi,
  UserApi,
  WorkspaceApi,
  DefaultConfig
} from "../../../generated";
import React, {createContext, PropsWithChildren} from "react";

export type Services = {
  authService: AuthApi,
  organizationService: OrganizationApi,
  subscriptionService: SubscriptionApi,
  transactionService: TransactionApi,
  transactionCategoryService: TransactionCategoryApi,
  userService: UserApi,
  workspaceService: WorkspaceApi
};

export function createServices(config?: Configuration | null): Services {
  const configuration = config ?? DefaultConfig;

  return {
    authService: new AuthApi(configuration),
    organizationService: new OrganizationApi(configuration),
    subscriptionService: new SubscriptionApi(configuration),
    transactionService: new TransactionApi(configuration),
    transactionCategoryService: new TransactionCategoryApi(configuration),
    userService: new UserApi(configuration),
    workspaceService: new WorkspaceApi(configuration)
  };
}

export const ServicesContext = createContext<Services>(null!);

export type ServicesProviderProps = {
  services: Services
};

export default function ServicesProvider(props: PropsWithChildren<ServicesProviderProps>): React.ReactNode {
  return (
    <ServicesContext.Provider value={props.services}>
      {props.children}
    </ServicesContext.Provider>
  );
}
