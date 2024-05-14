FROM openapitools/openapi-generator-cli

CMD /usr/local/bin/docker-entrypoint.sh generate \
		-i /openapi/index.yaml \
        -g typescript-angular \
        -o /frontend/generated
