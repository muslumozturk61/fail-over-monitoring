echo "started"
docker compose -f .\docker-compose.yml down
echo "containers stoped"

docker rmi nginx-lb

echo "nginx-lb image removed"
 
docker compose -f .\docker-compose.yml up -d
echo "containers started"