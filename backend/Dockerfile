FROM postgres:latest

# Set the environment variables
ENV POSTGRES_USER=postgres
ENV POSTGRES_PASSWORD=debianvm2
ENV POSTGRES_DB=PvPet

RUN apt-get update \
    && apt-get install -y postgis

# Copy the SQL script to the appropriate location
COPY ./init.sql /docker-entrypoint-initdb.d/

# Ensure the script has the correct permissions
RUN chmod 755 /docker-entrypoint-initdb.d/init.sql