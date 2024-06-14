#!/bin/bash
set -eoux pipefail

SCRIPT_ROOT=$(dirname "${BASH_SOURCE[0]}")
PROJECT_ROOT="${SCRIPT_ROOT}/.."
CONTAINER_DATA_DIR="${PROJECT_ROOT}/container_data"
DATE_FOR_FILES=$(date +'%Y-%m-%d_%H_%M_%S')
# Go to where the directory with the docker compose file is
cd "${PROJECT_ROOT}"
pwd

docker compose --profile all down
sleep 3
stat "${CONTAINER_DATA_DIR}/unleash_postgres" && mv "${CONTAINER_DATA_DIR}/unleash_postgres" "${CONTAINER_DATA_DIR}/${DATE_FOR_FILES}-unleash_postgres"
stat "${CONTAINER_DATA_DIR}/dreamer_postgres" && mv "${CONTAINER_DATA_DIR}/dreamer_postgres" "${CONTAINER_DATA_DIR}/${DATE_FOR_FILES}-dreamer_postgres"

docker compose --profile all up unleash_postgres dreamer_postgres -d
sleep 15 # Give it enough time to start up

gunzip -c "${SCRIPT_ROOT}/data/unleash_postgres_dump.gz" | docker exec -i unleash_postgres psql -U root -d unleash
sleep 2
gunzip -c "${SCRIPT_ROOT}/data/dreamer_postgres_dump.gz" | docker exec -i dreamer_postgres psql -U root -d dreamer
