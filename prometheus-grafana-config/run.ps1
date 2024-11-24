echo "started"
docker compose -f .\docker-compose.yml down
echo "containers stoped"
 
docker compose -f .\docker-compose.yml up -d
echo "containers started"