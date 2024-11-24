echo "started"
docker compose -f .\docker-compose.yml down
docker compose -f .\docker-compose-new.yml down

echo "containers stoped"

docker rmi sample-api

echo "sample-api image removed"
 
docker compose -f .\docker-compose.yml up -d
docker compose -f .\docker-compose-new.yml up -d

echo "containers started"