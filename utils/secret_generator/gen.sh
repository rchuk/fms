#!/bin/bash

filename=/out/secrets.env

password_salt=$(openssl rand -base64 16)
jwt_secret=$(openssl rand -base64 32)
pbkdf_iterations=100001

if [ ! -f ${filename} ]; then
	echo "Creating secrets file '${filename}'"
	printf "PASSWORD_SALT=${password_salt}\nJWT_SECRET=${jwt_secret}\nPBKDF_ITERATIONS=${pbkdf_iterations}" > ${filename}
else
	echo "Secrets file '${filename}' already exists"
fi
