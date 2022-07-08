# Reference Architecture

## Getting Started

### Prerequisites

Installation and working knowledge of the following are required:

- .NET6 - `dotnet --version` must return `6.0.301`, or greater
- Docker - `docker --version` - tested with `20.10.14`
- Git - `git --version` - tested with `2.34.1`

### Run the Application

- Git Clone: `git clone TODO`
- CD into the project's src folder `cd TODO/src`
- [Start Kafka On Your Machine](#run-local-kafka)
- Linux! If using Windows, you will need to adjust commands.

## Development

### Run Local Kafka

This solution is configured to use a local instance of Kafka. You may reconfigure it to use any cluster you like. To get started, leave everything as is, and start Kafka docker containers.

The `docker-compose.yml` file defines a Kafka broker and Zookeeper instance. It is a subset of the [Confluent All-In-One Dockerfile](https://github.com/confluentinc/cp-all-in-one/blob/7.1.1-post/cp-all-in-one/docker-compose.yml).


```bash
# go to the folder that contains the docker-compose file
# alternatively, you can run from anywhere by using the `-f` parameter
cd src/Documentation/Docker/Kafka

# OPTIONAL: If you have any issues starting the containers, it could be because they terminated ungracefully.
# this will usually correct the issue:
docker-compose rm -f

# start the containers
docker-compose up -d

# view the containers
docker-compose ps

# restart (stop/start) the containers
docker-compose restart

# view the containers
docker-compose ps
````



### Docker/Docker-Compose Quick Reference

These are common commands that I use when working with Docker. This only represents a tiny fraction of what docker is capable of. Consult the docker documentation.

NOTE: A `volume` is disk space that can be made available to 0 or more containers. When the containers stop, the data persists. New containers may be attached to the same volume.

Beware that when you delete a volume you are deleting data.

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
docker system prune -a --volumes

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




