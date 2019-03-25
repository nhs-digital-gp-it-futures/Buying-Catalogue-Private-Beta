FROM index.docker.io/node:10 AS base
WORKDIR /home/node/app

FROM base AS dependencies
COPY package.json .
COPY lib ./lib
RUN npm install

FROM dependencies AS pre-build
COPY src ./src

FROM pre-build AS test
#COPY __mocks__ __mocks__/
#COPY __tests__ __tests__/
#COPY jest.setup.js .
RUN npm test

FROM pre-build AS build
RUN npm run build

FROM index.docker.io/node:10 AS release
COPY --from=dependencies /home/node/app/package.json .
COPY --from=dependencies /home/node/app/lib ./lib
RUN npm install --production
COPY --from=build /home/node/app/src ./src
RUN adduser --disabled-password app
USER app
CMD ["npm", "start"]
