#!/bin/bash
set -e

until /opt/mssql-tools/bin/sqlcmd -S db -U sa -P sa -Q "SELECT 1" > /dev/null 2>&1
do
  sleep 2
done

dotnet ef database update --context AppDbContext --no-build

dotnet ef database update --context BrbTechDbContext --no-build

exec dotnet WebAPI.dll