# CoreShop API


# Project Setup with Docker Compose

Welcome to my project! This guide will help you get started with running my application using Docker Compose. Docker Compose allows you to configure and start all the services defined in the `docker-compose.yml` file with just a few commands.

## Prerequisites

Before proceeding, make sure you have the following installed on your system:

- **Docker**: Docker must be installed and running. You can check if Docker is installed by running `docker --version` in your terminal. If you need to install Docker, please refer to the [official Docker documentation](https://docs.docker.com/get-docker/).

- **Docker Compose**: Docker Compose is required for orchestrating the multi-container setup. Verify its installation with `docker-compose --version`. For installation instructions, visit the [Docker Compose documentation](https://docs.docker.com/compose/install/).

## Getting Started


1. **Start the Services**

   Use Docker Compose to start the services defined in your `docker-compose.yml` file.

   ```bash
   docker-compose up
   ```

   Add the `-d` flag to run the services in detached mode (in the background):

   ```bash
   docker-compose up -d
   ```

2. **Access the Application**

   Once the services are up and running, you can access the application via the URLs provided in the `docker-compose.yml` file or as documented in the project.

## Stopping Services

To stop and remove all the running services, use the following command:

```bash
docker-compose down
```

## Additional Commands

- **View Running Containers**: To see a list of all running containers, you can use:

  ```bash
  docker-compose ps
  ```

- **View Logs**: For real-time logs of all services, execute:

  ```bash
  docker-compose logs -f
  ```

Replace `-f` with the name of a specific service to view logs for that service only.


### Creating new db migration
* Ensure Entity Framework tools are installed `dotnet tool install --global dotnet-ef`
* In project root catalog run command `dotnet ef migrations add [MIGRATION_NAME] -s API -p Domain  --context ShopDbContext`   