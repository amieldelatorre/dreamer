FROM --platform=$BUILDPLATFORM nginx as base
WORKDIR /usr/share/nginx/html
RUN rm -rf ./*
EXPOSE 80

FROM --platform=$BUILDPLATFORM node as build
WORKDIR /src
COPY ["package.json", "package.json"]
COPY ["package-lock.json", "package-lock.json"]
RUN npm install
COPY . .
RUN npm run build

FROM base as final
COPY --from=build /src/dist .
COPY [ "nginx.conf", "/etc/nginx/conf.d/default.conf" ]  
ENTRYPOINT [ "nginx", "-g", "daemon off;" ]
