#!/bin/bash
set -eoux pipefail

trap 'catch $? $LINENO' ERR

catch() {
  echo "Error $1 occured on line $2"
  docker compose --profile all down
  exit 1
}

SCRIPT_ROOT=$(dirname "${BASH_SOURCE[0]}")
PROJECT_ROOT="${SCRIPT_ROOT}/.."
# Go to where the directory with the docker compose file is
cd "${PROJECT_ROOT}"
pwd

docker network create dreamer

bash "${SCRIPT_ROOT}/restore_dumps.sh"
docker compose --profile all up -d
sleep 5

docker build -t selenium_test --progress=plain ./frontend/web_tests/
docker run --rm --network=host selenium_test
docker compose --profile all down
