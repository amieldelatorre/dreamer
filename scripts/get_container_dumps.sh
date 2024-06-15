#!/bin/bash
set -eoxu pipefail

SCRIPT_ROOT=$(dirname "${BASH_SOURCE[0]}")
PROJECT_ROOT="${SCRIPT_ROOT}/.."
# Go to where the directory with the docker compose file is
cd "${PROJECT_ROOT}"
pwd

docker compose --profile all up unleash_postgres dreamer_postgres -d 
sleep 5

# date_for_files=$(date +'%Y-%m-%d_%H_%M_%S')
# compressed files for general archiving
# uncompressed files for use in tests
unleash_postgres_dump_compressed_filename="${SCRIPT_ROOT}/data/unleash_postgres_dump.gz"
unleash_postgres_dump_filename="${PROJECT_ROOT}/backend/Dreamer.Tests/Data/unleash_postgres_dump.sql" 

dreamer_postgres_dump_compressed_filename="${SCRIPT_ROOT}/data/dreamer_postgres_dump.gz"
dreamer_postgres_dump_filename="${PROJECT_ROOT}/backend/Dreamer.Tests/Data/dreamer_postgres_dump.sql"


# pgdump_all -c # Includes SQL commands to clean (drop) databases before recreating them.
docker exec -t unleash_postgres pg_dump -U root -d unleash --inserts | tee "${unleash_postgres_dump_filename}" | gzip > "${unleash_postgres_dump_compressed_filename}"
sleep 2
docker exec -t dreamer_postgres pg_dump -U root -d dreamer --inserts | tee "${dreamer_postgres_dump_filename}" | gzip > "${dreamer_postgres_dump_compressed_filename}"
