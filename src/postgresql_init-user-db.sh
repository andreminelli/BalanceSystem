#!/bin/bash
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
	CREATE USER balance_user WITH ENCRYPTED PASSWORD 'us3r_P4ss';
	CREATE DATABASE "Balance";
	GRANT ALL PRIVILEGES ON DATABASE Balance TO balance_user;
EOSQL