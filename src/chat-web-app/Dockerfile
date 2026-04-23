FROM node:20-alpine AS build
WORKDIR /app

COPY *.* ./
RUN npm ci

COPY . .

# Build with a placeholder — substituted at container startup via entrypoint
ENV VITE_API_BASE_URL=__VITE_API_BASE_URL__
RUN npm run build

FROM nginx:alpine AS final
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Default value — override at runtime with -e VITE_API_BASE_URL=https://api.myapp.com
ENV VITE_API_BASE_URL=http://localhost:5066

COPY docker-entrypoint.sh /docker-entrypoint.sh
RUN chmod +x /docker-entrypoint.sh

EXPOSE 80
ENTRYPOINT ["/docker-entrypoint.sh"]
CMD ["nginx", "-g", "daemon off;"]
