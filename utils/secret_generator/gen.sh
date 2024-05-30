#!/bin/bash

filename=/out/secrets.json

password_salt=$(openssl rand -base64 16)
jwt_secret=$(openssl rand -base64 32)
pbkdf_iterations=100001

if [ ! -f ${filename} ]; then
	echo "Creating secrets file '${filename}'"
	printf "{\n\t\"Secrets\": {\n\t\t\"PasswordSalt\": \"${password_salt}\",\n\t\t\"JwtSecret\": \"${jwt_secret}\"\n\t}\n}" > ${filename}
else
	echo "Secrets file '${filename}' already exists"
fi
