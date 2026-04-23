#!/bin/sh
set -e

# Replace the build-time placeholder with the runtime VITE_API_BASE_URL value.
# This allows the image to be built once and configured per environment.
find /usr/share/nginx/html -name '*.js' -exec \
    sed -i "s|__VITE_API_BASE_URL__|${VITE_API_BASE_URL}|g" {} \;

exec "$@"
