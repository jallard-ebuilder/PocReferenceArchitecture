# Docker Cheat Sheet

- [Docker Cheat Sheet](#docker-cheat-sheet)

These are common commands that I use when working with Docker. This only represents a tiny fraction of what docker is capable of.

The `-f` parameter prevents confirmation prompts.

```bash
# -------------------------------------
# containers
# -------------------------------------
# List running containers
docker ps

# List all containers
docker ps -a

# Remove all stopped containers
docker rm $(docker ps --filter status=exited -q)

# -------------------------------------
# volumes
# -------------------------------------
# List volumes
docker volume ls

# List volumes, including disk size
docker system df -v
  
# Delete unused volumes
docker volume prune -f`

# -------------------------------------
# Prune Everything that's Not Used
# -------------------------------------
docker system prune --volumes -f

# -------------------------------------
# Docker compose
# -------------------------------------
# use the compose file in the current folder
docker-compose up

# use a specific compose file
docker-compose -f folder/compose.yml up

# run in detached mode - the containers will run in the background. usually, this is what you want.
docker compose up -d

# stop all the containers
docker-compose stop

# list all containers from the docker-compose
docker-compose ps
```

You may also prune everything at once: [docker system prune](https://docs.docker.com/engine/reference/commandline/system_prune/#examples)
