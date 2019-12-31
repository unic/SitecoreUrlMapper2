FROM node:12.13

# Install project dependencies
# -------------------------------------
WORKDIR /app
COPY package*.json ./
# this is needed so postinstall script is run (even as root user) https://geedew.com/What-does-unsafe-perm-in-npm-actually-do/
RUN npm config set unsafe-perm true
RUN npm ci

# Copy the entire project
# --------------------------------
# This layer is rebuilt when a file changes in the project directory
COPY . /app

# Define the main entrypoint in shell form
# -------------------------------------------------
ENTRYPOINT exec npm run build