FROM nginx:mainline-alpine

ARG SERVER_NAME=solr

ENV SSL_CERTIFICATE_NAME=urlmapper2-solr
ENV SSL_CERTIFICATE=/etc/ssl/certs/${SSL_CERTIFICATE_NAME}-ssl.keystore.cer
ENV SSL_CERTIFICATE_PFX=/etc/ssl/certs/${SSL_CERTIFICATE_NAME}-ssl.keystore.pfx
ENV SSL_CERTIFICATE_KEY=/etc/ssl/private/${SSL_CERTIFICATE_NAME}-ssl.keystore.key

RUN apk add --no-cache openssl \
    && openssl ecparam -out ${SSL_CERTIFICATE_KEY} -name prime256v1 -genkey \
    && /bin/echo "[ req ]" > /etc/ssl/certConfig.cnf \
    && /bin/echo "prompt = no" >> /etc/ssl/certConfig.cnf \
    && /bin/echo "x509_extensions = san_self_signed" >> /etc/ssl/certConfig.cnf \
    && /bin/echo "distinguished_name = req_distinguished_name" >> /etc/ssl/certConfig.cnf \
    && /bin/echo "[ req_distinguished_name ]" >> /etc/ssl/certConfig.cnf \
    && /bin/echo "countryName = US" >> /etc/ssl/certConfig.cnf \
    && /bin/echo "CN = ${SERVER_NAME}" >> /etc/ssl/certConfig.cnf \
    && /bin/echo "[ san_self_signed ]" >> /etc/ssl/certConfig.cnf \
    && /bin/echo "subjectAltName = DNS:${SERVER_NAME}, DNS:localhost" >> /etc/ssl/certConfig.cnf \
    && /bin/echo "subjectKeyIdentifier = hash" >> /etc/ssl/certConfig.cnf \
    && /bin/echo "authorityKeyIdentifier = keyid:always,issuer" >> /etc/ssl/certConfig.cnf \
    && /bin/echo "basicConstraints = CA:true" >> /etc/ssl/certConfig.cnf \
    && /bin/echo "keyUsage = nonRepudiation, digitalSignature, keyEncipherment, dataEncipherment, keyCertSign, cRLSign" >> /etc/ssl/certConfig.cnf \
    && /bin/echo "extendedKeyUsage = serverAuth, clientAuth, timeStamping" >> /etc/ssl/certConfig.cnf \
    && /bin/echo "" >> /etc/ssl/certConfig.cnf \
    && /bin/echo "" >> /etc/ssl/certConfig.cnf \
    && /bin/echo /etc/ssl/certConfig.cnf \
    && openssl req -new -key ${SSL_CERTIFICATE_KEY} -x509 -sha256 -nodes \
        -days 3650 -subj "/CN=${SERVER_NAME}" -config < /bin/echo /etc/ssl/certConfig.cnf \
        -out ${SSL_CERTIFICATE}

#  Convert the .cer file into .pfx format:
RUN openssl pkcs12 -export -name ${SSL_CERTIFICATE_NAME} -out ${SSL_CERTIFICATE_PFX} -inkey ${SSL_CERTIFICATE_KEY} -in ${SSL_CERTIFICATE} -passout pass:

RUN rm /etc/nginx/conf.d/*.conf

COPY . /

ENTRYPOINT ["/docker-entrypoint.sh"]