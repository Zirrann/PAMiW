# Etap 1: Build
FROM node:20 AS build
WORKDIR /app

# Instalacja zależności
COPY package*.json ./
RUN npm install

# Budowanie aplikacji
COPY . ./
RUN npm run build

# Etap 2: Serwowanie aplikacji z Nginx
FROM nginx:alpine
COPY --from=build /app/build /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
