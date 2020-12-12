dotnet ef dbcontext scaffold \
    "Server=$PGHOST;User Id=$PGUSER;Password=$PGPASSWORD;Database=$PGDATABASE" \
    "Npgsql.EntityFrameworkCore.PostgreSQL" \
    -o Models \
    --no-onconfiguring \
    --schema public \
    --context ApiContext \
    --force
