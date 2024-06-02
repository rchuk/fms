FROM openapitools/openapi-generator-cli

CMD /usr/local/bin/docker-entrypoint.sh generate \
		-i /openapi/index.yaml \
        -g typescript-fetch \
        -o /frontend/generated
