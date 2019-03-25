#!/bin/sh

# not really intended to be run as a script although it can, but these are the commands
# user to regenerate the API client code from Swagger when it changes.

rm -rf swagger_temp
mkdir swagger_temp
cp lib/catalogue-api/swagger.json swagger_temp/
docker run --rm -v ${PWD}:/local swaggerapi/swagger-codegen-cli:unstable generate -i ./local/swagger_temp/swagger.json -l javascript -o /local/swagger_temp --additional-properties usePromises=true,projectName=catalogue-api -t/local/utils/templates
rm -rf lib/catalogue-api
mv swagger_temp lib/catalogue-api
