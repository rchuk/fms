# syntax=docker/dockerfile:1

FROM alpine:3.14

WORKDIR /app

RUN apk add openssl

COPY gen.sh .
RUN chmod +x gen.sh

ENTRYPOINT ["sh", "gen.sh"]
