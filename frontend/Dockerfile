# syntax=docker/dockerfile:1

FROM --platform=$BUILDPLATFORM node:18.17.1-bullseye-slim as builder

RUN mkdir /app
WORKDIR /app

RUN npm install -g @angular/cli@17

COPY package.json package-lock.json ./
RUN npm ci

COPY . .
CMD ["ng", "serve", "--host", "0.0.0.0"]
