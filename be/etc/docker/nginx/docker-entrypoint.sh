#!/bin/sh

mkdir /certs -p
cp /etc/ssl/certs/urlmapper2-solr-ssl.keystore.cer /certs/
cp /etc/ssl/private/urlmapper2-solr-ssl.keystore.key /certs/
cp /etc/ssl/certs/urlmapper2-solr-ssl.keystore.pfx /certs/

cat > /etc/nginx/conf.d/proxy.conf << EOT
map \$http_upgrade \$connection_upgrade {
    default upgrade;
    ''      close;
}

server {
    listen 443 ssl;
    
    server_name ${SERVER_NAME:-_};
    ssl_certificate /etc/ssl/certs/urlmapper2-solr-ssl.keystore.cer;
    ssl_certificate_key /etc/ssl/private/urlmapper2-solr-ssl.keystore.key;
    client_max_body_size 100M;
    
    location / {

        proxy_pass ${PROXY_PASS:-http://upstream};
        proxy_redirect ${PROXY_REDIRECT:-default};
        proxy_set_header Host ${PROXY_HOST:-\$host};
        proxy_set_header X-Forwarded-For \$proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Host \$http_host;
        proxy_set_header X-Forwarded-Proto \$scheme;
    }
}

server {
    listen 80 default;

    server_name ${SERVER_NAME:-_};
    return 301 https://\$server_name\$request_uri;
}

# Up the buffer sizes, as otherwise we get "upstream sent too big header while reading response header from upstream" errors
# ref https://stackoverflow.com/a/27551259
proxy_buffer_size		128k;
proxy_buffers			4 256k;
proxy_busy_buffers_size 256k;
proxy_read_timeout      300s;

EOT

echo "Starting nginx"
exec nginx -g 'daemon off;'