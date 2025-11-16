# Docker & CI/CD Complete Guide
## From Basics to Production-Ready Pipelines

---

## Table of Contents

### Part A: Docker
1. [Docker Fundamentals](#1-docker-fundamentals)
2. [Dockerfile Best Practices](#2-dockerfile-best-practices)
3. [Docker Compose](#3-docker-compose)
4. [Docker Networking](#4-docker-networking)
5. [Docker Volumes & Data](#5-docker-volumes--data)
6. [Multi-Stage Builds](#6-multi-stage-builds)
7. [Docker Security](#7-docker-security)
8. [Docker in Production](#8-docker-in-production)

### Part B: CI/CD
9. [CI/CD Fundamentals](#9-cicd-fundamentals)
10. [GitHub Actions](#10-github-actions)
11. [Azure DevOps Pipelines](#11-azure-devops-pipelines)
12. [GitLab CI/CD](#12-gitlab-cicd)
13. [Jenkins Pipelines](#13-jenkins-pipelines)
14. [Deployment Strategies](#14-deployment-strategies)
15. [Pipeline Best Practices](#15-pipeline-best-practices)
16. [Real-World Examples](#16-real-world-examples)

---

## PART A: DOCKER

## 1. Docker Fundamentals

### 1.1 What is Docker?

Docker is a platform for developing, shipping, and running applications in containers.

**Key Concepts:**
- **Container**: Lightweight, standalone executable package
- **Image**: Read-only template for creating containers
- **Dockerfile**: Script to build Docker images
- **Registry**: Storage for Docker images (Docker Hub, ACR, ECR)

### 1.2 Basic Docker Commands

```bash
# ============================================
# IMAGE COMMANDS
# ============================================

# Pull image from registry
docker pull nginx:latest
docker pull mcr.microsoft.com/dotnet/sdk:8.0

# List images
docker images
docker image ls

# Remove image
docker rmi nginx:latest
docker image rm nginx:latest

# Remove unused images
docker image prune
docker image prune -a  # Remove all unused images

# Build image from Dockerfile
docker build -t myapp:1.0 .
docker build -t myapp:latest -f Dockerfile.prod .

# Tag image
docker tag myapp:1.0 myregistry.azurecr.io/myapp:1.0

# Push image to registry
docker push myregistry.azurecr.io/myapp:1.0

# Save/Load images
docker save myapp:1.0 > myapp.tar
docker load < myapp.tar

# ============================================
# CONTAINER COMMANDS
# ============================================

# Run container
docker run nginx
docker run -d nginx                    # Detached mode
docker run -d -p 8080:80 nginx        # Port mapping
docker run -d --name mynginx nginx    # Named container
docker run -it ubuntu bash            # Interactive terminal

# List containers
docker ps                             # Running containers
docker ps -a                          # All containers
docker ps -q                          # Only container IDs

# Stop/Start containers
docker stop mynginx
docker start mynginx
docker restart mynginx

# Remove container
docker rm mynginx
docker rm -f mynginx                  # Force remove running container

# View logs
docker logs mynginx
docker logs -f mynginx                # Follow logs
docker logs --tail 100 mynginx        # Last 100 lines

# Execute command in running container
docker exec mynginx ls /usr/share/nginx/html
docker exec -it mynginx bash          # Interactive shell

# Copy files
docker cp myfile.txt mynginx:/usr/share/nginx/html/
docker cp mynginx:/var/log/nginx/access.log ./

# Inspect container
docker inspect mynginx
docker stats mynginx                  # Resource usage

# ============================================
# CLEANUP COMMANDS
# ============================================

# Remove all stopped containers
docker container prune

# Remove all unused containers, networks, images
docker system prune
docker system prune -a --volumes      # Including volumes

# View disk usage
docker system df
```

### 1.3 Docker Run Options

```bash
# Common options
docker run \
  -d \                                    # Detached mode
  --name myapp \                          # Container name
  -p 8080:80 \                           # Port mapping (host:container)
  -e ENV_VAR=value \                     # Environment variable
  -v /host/path:/container/path \        # Volume mount
  --network mynetwork \                  # Connect to network
  --restart unless-stopped \             # Restart policy
  --memory="512m" \                      # Memory limit
  --cpus="1.5" \                        # CPU limit
  --health-cmd="curl -f http://localhost || exit 1" \
  --health-interval=30s \
  myapp:latest

# Restart policies
docker run --restart=no               # Never restart (default)
docker run --restart=on-failure       # Restart on failure
docker run --restart=always           # Always restart
docker run --restart=unless-stopped   # Restart unless manually stopped
```

---

## 2. Dockerfile Best Practices

### 2.1 Basic Dockerfile Structure

```dockerfile
# Syntax version (optional but recommended)
# syntax=docker/dockerfile:1

# Base image
FROM node:18-alpine

# Metadata
LABEL maintainer="your.email@example.com"
LABEL version="1.0"
LABEL description="My Node.js Application"

# Set working directory
WORKDIR /app

# Copy dependency files first (for layer caching)
COPY package*.json ./

# Install dependencies
RUN npm ci --only=production

# Copy application code
COPY . .

# Expose port
EXPOSE 3000

# Set environment variables
ENV NODE_ENV=production

# Create non-root user
RUN addgroup -g 1001 -S nodejs && \
    adduser -S nodejs -u 1001

# Change ownership
RUN chown -R nodejs:nodejs /app

# Switch to non-root user
USER nodejs

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD node healthcheck.js

# Default command
CMD ["node", "server.js"]
```

### 2.2 .NET Application Dockerfile

```dockerfile
# Multi-stage build for .NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["MyApp/MyApp.csproj", "MyApp/"]
RUN dotnet restore "MyApp/MyApp.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/MyApp"
RUN dotnet build "MyApp.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "MyApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy published app
COPY --from=publish /app/publish .

# Run as non-root
USER app

ENTRYPOINT ["dotnet", "MyApp.dll"]
```

### 2.3 Python Application Dockerfile

```dockerfile
FROM python:3.11-slim

# Set environment variables
ENV PYTHONDONTWRITEBYTECODE=1 \
    PYTHONUNBUFFERED=1 \
    PIP_NO_CACHE_DIR=1 \
    PIP_DISABLE_PIP_VERSION_CHECK=1

WORKDIR /app

# Install system dependencies
RUN apt-get update && \
    apt-get install -y --no-install-recommends gcc && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Copy requirements
COPY requirements.txt .

# Install Python dependencies
RUN pip install --no-cache-dir -r requirements.txt

# Copy application
COPY . .

# Create non-root user
RUN useradd -m -u 1000 appuser && \
    chown -R appuser:appuser /app

USER appuser

EXPOSE 8000

CMD ["gunicorn", "--bind", "0.0.0.0:8000", "app:app"]
```

### 2.4 React/Next.js Dockerfile

```dockerfile
FROM node:18-alpine AS deps
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm ci

FROM node:18-alpine AS builder
WORKDIR /app
COPY --from=deps /app/node_modules ./node_modules
COPY . .
RUN npm run build

FROM node:18-alpine AS runner
WORKDIR /app

ENV NODE_ENV production

RUN addgroup --system --gid 1001 nodejs
RUN adduser --system --uid 1001 nextjs

COPY --from=builder /app/public ./public
COPY --from=builder --chown=nextjs:nodejs /app/.next/standalone ./
COPY --from=builder --chown=nextjs:nodejs /app/.next/static ./.next/static

USER nextjs

EXPOSE 3000

ENV PORT 3000

CMD ["node", "server.js"]
```

### 2.5 Optimization Techniques

```dockerfile
# ============================================
# 1. Layer Caching - Order matters!
# ============================================

# BAD: Changes to code invalidate all layers
FROM node:18
WORKDIR /app
COPY . .                    # Everything copied
RUN npm install            # Reinstalls every time code changes

# GOOD: Dependencies cached separately
FROM node:18
WORKDIR /app
COPY package*.json ./      # Only dependency files
RUN npm install           # Cached if package.json unchanged
COPY . .                  # Code changes don't affect npm install

# ============================================
# 2. Multi-line RUN Commands
# ============================================

# BAD: Each RUN creates a layer
RUN apt-get update
RUN apt-get install -y git
RUN apt-get install -y curl
RUN rm -rf /var/lib/apt/lists/*

# GOOD: Single layer
RUN apt-get update && \
    apt-get install -y \
        git \
        curl && \
    rm -rf /var/lib/apt/lists/*

# ============================================
# 3. .dockerignore File
# ============================================

# Create .dockerignore file:
node_modules
npm-debug.log
.git
.gitignore
README.md
.env
.vscode
dist
coverage
*.md
.DS_Store

# ============================================
# 4. Use Specific Image Tags
# ============================================

# BAD
FROM node:latest

# GOOD
FROM node:18.17.1-alpine

# ============================================
# 5. Minimize Image Size
# ============================================

# Use alpine versions (smaller)
FROM node:18-alpine          # ~180MB
FROM python:3.11-slim        # ~120MB
FROM node:18                 # ~1GB

# Clean up in same layer
RUN apt-get update && \
    apt-get install -y package && \
    rm -rf /var/lib/apt/lists/*

# Use multi-stage builds
FROM node:18 AS builder
# ... build steps ...

FROM node:18-alpine
COPY --from=builder /app/dist ./dist
```

### 2.6 Security Best Practices

```dockerfile
# ============================================
# Security Best Practices
# ============================================

FROM node:18-alpine

# 1. Run as non-root user
RUN addgroup -g 1001 -S nodejs && \
    adduser -S nodejs -u 1001

# 2. Set working directory
WORKDIR /app

# 3. Copy files with proper ownership
COPY --chown=nodejs:nodejs package*.json ./

# 4. Install dependencies
RUN npm ci --only=production && \
    npm cache clean --force

# 5. Copy application code
COPY --chown=nodejs:nodejs . .

# 6. Switch to non-root user
USER nodejs

# 7. Expose only necessary ports
EXPOSE 3000

# 8. Don't store secrets in image
# Use --build-arg for build-time secrets
ARG BUILD_SECRET
RUN echo "Building with secret..."
# Secret not stored in final image

# 9. Use HEALTHCHECK
HEALTHCHECK --interval=30s --timeout=3s \
  CMD node healthcheck.js || exit 1

# 10. Use read-only root filesystem
# docker run --read-only myapp

CMD ["node", "server.js"]
```

---

## 3. Docker Compose

### 3.1 Basic docker-compose.yml

```yaml
version: '3.8'

services:
  # Web application
  web:
    build:
      context: .
      dockerfile: Dockerfile
    ports:
      - "3000:3000"
    environment:
      - NODE_ENV=production
      - DB_HOST=db
    depends_on:
      - db
      - redis
    networks:
      - app-network
    restart: unless-stopped

  # Database
  db:
    image: postgres:15-alpine
    environment:
      - POSTGRES_DB=myapp
      - POSTGRES_USER=admin
      - POSTGRES_PASSWORD=secret123
    volumes:
      - postgres-data:/var/lib/postgresql/data
      - ./init.sql:/docker-entrypoint-initdb.d/init.sql
    networks:
      - app-network
    restart: unless-stopped

  # Redis cache
  redis:
    image: redis:7-alpine
    ports:
      - "6379:6379"
    networks:
      - app-network
    restart: unless-stopped

networks:
  app-network:
    driver: bridge

volumes:
  postgres-data:
```

### 3.2 Full-Stack Application Example

```yaml
version: '3.8'

services:
  # Frontend - React
  frontend:
    build:
      context: ./frontend
      dockerfile: Dockerfile
      args:
        - REACT_APP_API_URL=http://localhost:5000
    ports:
      - "3000:3000"
    volumes:
      - ./frontend:/app
      - /app/node_modules
    environment:
      - NODE_ENV=development
    depends_on:
      - backend
    networks:
      - app-network

  # Backend - .NET API
  backend:
    build:
      context: ./backend
      dockerfile: Dockerfile
    ports:
      - "5000:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Server=db;Database=myapp;User=sa;Password=YourStrong@Passw0rd;
      - Redis__Host=redis
    depends_on:
      - db
      - redis
    networks:
      - app-network
    restart: unless-stopped

  # Database - SQL Server
  db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourStrong@Passw0rd
      - MSSQL_PID=Express
    ports:
      - "1433:1433"
    volumes:
      - sqlserver-data:/var/opt/mssql
    networks:
      - app-network
    restart: unless-stopped

  # Cache - Redis
  redis:
    image: redis:7-alpine
    command: redis-server --appendonly yes
    ports:
      - "6379:6379"
    volumes:
      - redis-data:/data
    networks:
      - app-network
    restart: unless-stopped

  # Message Queue - RabbitMQ
  rabbitmq:
    image: rabbitmq:3-management-alpine
    ports:
      - "5672:5672"
      - "15672:15672"
    environment:
      - RABBITMQ_DEFAULT_USER=admin
      - RABBITMQ_DEFAULT_PASS=secret
    volumes:
      - rabbitmq-data:/var/lib/rabbitmq
    networks:
      - app-network
    restart: unless-stopped

  # Nginx - Reverse Proxy
  nginx:
    image: nginx:alpine
    ports:
      - "80:80"
      - "443:443"
    volumes:
      - ./nginx/nginx.conf:/etc/nginx/nginx.conf:ro
      - ./nginx/ssl:/etc/nginx/ssl:ro
    depends_on:
      - frontend
      - backend
    networks:
      - app-network
    restart: unless-stopped

networks:
  app-network:
    driver: bridge

volumes:
  sqlserver-data:
  redis-data:
  rabbitmq-data:
```

### 3.3 Docker Compose Commands

```bash
# ============================================
# Basic Commands
# ============================================

# Start services
docker-compose up
docker-compose up -d                    # Detached mode
docker-compose up --build              # Rebuild images
docker-compose up -d frontend backend  # Specific services

# Stop services
docker-compose stop
docker-compose stop frontend           # Specific service

# Stop and remove containers
docker-compose down
docker-compose down -v                 # Also remove volumes
docker-compose down --rmi all         # Also remove images

# View logs
docker-compose logs
docker-compose logs -f                # Follow logs
docker-compose logs -f frontend       # Specific service
docker-compose logs --tail=100        # Last 100 lines

# List containers
docker-compose ps
docker-compose ps -a

# Execute command
docker-compose exec backend bash
docker-compose exec db psql -U postgres

# Build images
docker-compose build
docker-compose build --no-cache       # No cache

# Pull images
docker-compose pull

# Restart services
docker-compose restart
docker-compose restart frontend

# Scale services
docker-compose up -d --scale worker=3

# Validate configuration
docker-compose config

# View resource usage
docker-compose top
```

### 3.4 Environment Variables

```yaml
# docker-compose.yml
version: '3.8'

services:
  web:
    build: .
    ports:
      - "${APP_PORT:-3000}:3000"
    environment:
      - NODE_ENV=${NODE_ENV}
      - DATABASE_URL=${DATABASE_URL}
    env_file:
      - .env
      - .env.local
```

```bash
# .env file
NODE_ENV=production
APP_PORT=3000
DATABASE_URL=postgresql://user:pass@db:5432/mydb
REDIS_URL=redis://redis:6379
```

### 3.5 Health Checks in Compose

```yaml
version: '3.8'

services:
  web:
    build: .
    ports:
      - "3000:3000"
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:3000/health"]
      interval: 30s
      timeout: 3s
      retries: 3
      start_period: 40s

  db:
    image: postgres:15
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U postgres"]
      interval: 10s
      timeout: 5s
      retries: 5

  redis:
    image: redis:7-alpine
    healthcheck:
      test: ["CMD", "redis-cli", "ping"]
      interval: 10s
      timeout: 3s
      retries: 3
```

---

## 4. Docker Networking

### 4.1 Network Types

```bash
# Bridge Network (default)
docker network create my-bridge-network
docker run -d --name web --network my-bridge-network nginx

# Host Network (shares host's network)
docker run -d --network host nginx

# None Network (no networking)
docker run -d --network none nginx

# Custom network
docker network create --driver bridge \
  --subnet=172.18.0.0/16 \
  --gateway=172.18.0.1 \
  my-custom-network

# List networks
docker network ls

# Inspect network
docker network inspect my-bridge-network

# Connect container to network
docker network connect my-network my-container

# Disconnect
docker network disconnect my-network my-container

# Remove network
docker network rm my-network
```

### 4.2 Container Communication

```yaml
# docker-compose.yml
version: '3.8'

services:
  frontend:
    build: ./frontend
    networks:
      - frontend-network
    depends_on:
      - backend

  backend:
    build: ./backend
    networks:
      - frontend-network
      - backend-network
    environment:
      # Access database using service name
      - DB_HOST=database
    depends_on:
      - database

  database:
    image: postgres:15
    networks:
      - backend-network
    # Not accessible from frontend

networks:
  frontend-network:
  backend-network:
```

---

## 5. Docker Volumes & Data

### 5.1 Volume Types

```bash
# Named volumes (managed by Docker)
docker volume create my-volume
docker run -v my-volume:/app/data nginx

# Bind mounts (host filesystem)
docker run -v /host/path:/container/path nginx
docker run -v $(pwd):/app nginx

# Anonymous volumes
docker run -v /app/data nginx

# Read-only volumes
docker run -v my-volume:/app/data:ro nginx

# Volume commands
docker volume ls
docker volume inspect my-volume
docker volume rm my-volume
docker volume prune
```

### 5.2 Data Persistence

```yaml
version: '3.8'

services:
  db:
    image: postgres:15
    volumes:
      # Named volume for data persistence
      - postgres-data:/var/lib/postgresql/data
      # Bind mount for initialization scripts
      - ./init-scripts:/docker-entrypoint-initdb.d
      # Bind mount for config
      - ./postgres.conf:/etc/postgresql/postgresql.conf:ro

  app:
    build: .
    volumes:
      # Source code (development)
      - ./src:/app/src
      # Prevent overwriting node_modules
      - /app/node_modules
      # Logs
      - app-logs:/app/logs

volumes:
  postgres-data:
    driver: local
  app-logs:
    driver: local
```

### 5.3 Backup and Restore

```bash
# Backup volume
docker run --rm \
  -v my-volume:/data \
  -v $(pwd):/backup \
  alpine tar czf /backup/backup.tar.gz /data

# Restore volume
docker run --rm \
  -v my-volume:/data \
  -v $(pwd):/backup \
  alpine tar xzf /backup/backup.tar.gz -C /

# Backup database
docker exec my-postgres pg_dump -U postgres mydb > backup.sql

# Restore database
docker exec -i my-postgres psql -U postgres mydb < backup.sql
```

---

## 6. Multi-Stage Builds

### 6.1 Node.js Production Build

```dockerfile
# Stage 1: Build
FROM node:18 AS builder
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build
RUN npm prune --production

# Stage 2: Production
FROM node:18-alpine
WORKDIR /app
COPY --from=builder /app/dist ./dist
COPY --from=builder /app/node_modules ./node_modules
COPY --from=builder /app/package.json ./
USER node
EXPOSE 3000
CMD ["node", "dist/server.js"]
```

### 6.2 Go Application Build

```dockerfile
# Build stage
FROM golang:1.21-alpine AS builder
WORKDIR /build
COPY go.mod go.sum ./
RUN go mod download
COPY . .
RUN CGO_ENABLED=0 GOOS=linux go build -a -installsuffix cgo -o app .

# Final stage
FROM alpine:latest
RUN apk --no-cache add ca-certificates
WORKDIR /root/
COPY --from=builder /build/app .
EXPOSE 8080
CMD ["./app"]
```

---

## 7. Docker Security

### 7.1 Security Best Practices

```dockerfile
# ============================================
# Secure Dockerfile
# ============================================

FROM node:18-alpine AS builder

# Use specific versions
FROM node:18.17.1-alpine

# Create non-root user
RUN addgroup -g 1001 -S appgroup && \
    adduser -S appuser -u 1001 -G appgroup

WORKDIR /app

# Copy with ownership
COPY --chown=appuser:appgroup package*.json ./

# Install only production dependencies
RUN npm ci --only=production && \
    npm cache clean --force

COPY --chown=appuser:appgroup . .

# Drop privileges
USER appuser

# Use COPY instead of ADD
COPY file.txt /app/

# Don't expose unnecessary ports
EXPOSE 3000

# Use HEALTHCHECK
HEALTHCHECK CMD node healthcheck.js

CMD ["node", "server.js"]
```

### 7.2 Scanning Images

```bash
# Docker scan (deprecated, use Snyk/Trivy)
docker scan myapp:latest

# Trivy scan
trivy image myapp:latest
trivy image --severity HIGH,CRITICAL myapp:latest

# Snyk scan
snyk container test myapp:latest

# Hadolint (Dockerfile linter)
hadolint Dockerfile
```

### 7.3 Secrets Management

```bash
# Docker secrets (Swarm mode)
echo "my-secret-password" | docker secret create db_password -

# Use in service
docker service create \
  --name myapp \
  --secret db_password \
  myapp:latest

# In container, secret available at:
# /run/secrets/db_password
```

```yaml
# docker-compose.yml with secrets
version: '3.8'

services:
  db:
    image: postgres:15
    secrets:
      - db_password
    environment:
      POSTGRES_PASSWORD_FILE: /run/secrets/db_password

secrets:
  db_password:
    file: ./secrets/db_password.txt
```

---

## 8. Docker in Production

### 8.1 Resource Limits

```yaml
version: '3.8'

services:
  web:
    image: myapp:latest
    deploy:
      resources:
        limits:
          cpus: '0.50'
          memory: 512M
        reservations:
          cpus: '0.25'
          memory: 256M
      restart_policy:
        condition: on-failure
        delay: 5s
        max_attempts: 3
```

```bash
# Run with resource limits
docker run -d \
  --memory="512m" \
  --memory-swap="1g" \
  --cpus="1.5" \
  --pids-limit=100 \
  myapp:latest
```

### 8.2 Logging

```yaml
version: '3.8'

services:
  web:
    image: myapp:latest
    logging:
      driver: "json-file"
      options:
        max-size: "10m"
        max-file: "3"
        labels: "production"

  # Alternative: Send to syslog
  api:
    image: myapi:latest
    logging:
      driver: "syslog"
      options:
        syslog-address: "tcp://192.168.0.42:514"
        tag: "api"
```

### 8.3 Monitoring

```yaml
version: '3.8'

services:
  # Application
  app:
    build: .
    ports:
      - "3000:3000"

  # Prometheus
  prometheus:
    image: prom/prometheus:latest
    ports:
      - "9090:9090"
    volumes:
      - ./prometheus.yml:/etc/prometheus/prometheus.yml
      - prometheus-data:/prometheus

  # Grafana
  grafana:
    image: grafana/grafana:latest
    ports:
      - "3001:3000"
    volumes:
      - grafana-data:/var/lib/grafana
    environment:
      - GF_SECURITY_ADMIN_PASSWORD=admin

volumes:
  prometheus-data:
  grafana-data:
```

---

## PART B: CI/CD

## 9. CI/CD Fundamentals

### 9.1 CI/CD Pipeline Stages

```
1. Source → 2. Build → 3. Test → 4. Deploy

Continuous Integration (CI):
- Source control triggers build
- Automated tests run
- Code quality checks
- Build artifacts

Continuous Deployment (CD):
- Deploy to staging
- Run integration tests
- Deploy to production
- Monitor
```

### 9.2 Pipeline Best Practices

```yaml
# Good CI/CD Pipeline Structure
Pipeline:
  - Stage: Build
      - Checkout code
      - Install dependencies
      - Compile/Build
      - Create artifacts

  - Stage: Test
      - Unit tests
      - Integration tests
      - Code coverage
      - Security scan

  - Stage: Quality
      - Linting
      - Code analysis
      - Dependency check

  - Stage: Package
      - Build Docker image
      - Tag image
      - Push to registry

  - Stage: Deploy (Staging)
      - Deploy to staging
      - Run smoke tests

  - Stage: Deploy (Production)
      - Manual approval
      - Blue-green deployment
      - Health checks
      - Rollback on failure
```

---

## 10. GitHub Actions

### 10.1 Basic Workflow

```.yaml
# .github/workflows/ci.yml
name: CI/CD Pipeline

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main ]

env:
  NODE_VERSION: '18'
  REGISTRY: ghcr.io
  IMAGE_NAME: ${{ github.repository }}

jobs:
  # ============================================
  # Job 1: Build and Test
  # ============================================
  build-and-test:
    runs-on: ubuntu-latest

    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      - name: Setup Node.js
        uses: actions/setup-node@v4
        with:
          node-version: ${{ env.NODE_VERSION }}
          cache: 'npm'

      - name: Install dependencies
        run: npm ci

      - name: Run linter
        run: npm run lint

      - name: Run tests
        run: npm test

      - name: Run test coverage
        run: npm run test:coverage

      - name: Upload coverage reports
        uses: codecov/codecov-action@v3
        with:
          files: ./coverage/coverage-final.json

      - name: Build application
        run: npm run build

      - name: Upload build artifacts
        uses: actions/upload-artifact@v3
        with:
          name: build-output
          path: dist/

  # ============================================
  # Job 2: Build Docker Image
  # ============================================
  build-image:
    needs: build-and-test
    runs-on: ubuntu-latest
    permissions:
      contents: read
      packages: write

    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      - name: Set up Docker Buildx
        uses: docker/setup-buildx-action@v3

      - name: Log in to Container Registry
        uses: docker/login-action@v3
        with:
          registry: ${{ env.REGISTRY }}
          username: ${{ github.actor }}
          password: ${{ secrets.GITHUB_TOKEN }}

      - name: Extract metadata
        id: meta
        uses: docker/metadata-action@v5
        with:
          images: ${{ env.REGISTRY }}/${{ env.IMAGE_NAME }}
          tags: |
            type=ref,event=branch
            type=ref,event=pr
            type=semver,pattern={{version}}
            type=semver,pattern={{major}}.{{minor}}
            type=sha

      - name: Build and push Docker image
        uses: docker/build-push-action@v5
        with:
          context: .
          push: true
          tags: ${{ steps.meta.outputs.tags }}
          labels: ${{ steps.meta.outputs.labels }}
          cache-from: type=gha
          cache-to: type=gha,mode=max

  # ============================================
  # Job 3: Deploy to Staging
  # ============================================
  deploy-staging:
    needs: build-image
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/develop'
    environment:
      name: staging
      url: https://staging.example.com

    steps:
      - name: Deploy to staging
        run: |
          echo "Deploying to staging..."
          # Add deployment commands here

  # ============================================
  # Job 4: Deploy to Production
  # ============================================
  deploy-production:
    needs: build-image
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/main'
    environment:
      name: production
      url: https://example.com

    steps:
      - name: Deploy to production
        run: |
          echo "Deploying to production..."
          # Add deployment commands here
```

### 10.2 .NET Application Workflow

```yaml
# .github/workflows/dotnet.yml
name: .NET CI/CD

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

jobs:
  build-and-test:
    runs-on: ubuntu-latest

    steps:
      - uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0.x'

      - name: Restore dependencies
        run: dotnet restore

      - name: Build
        run: dotnet build --no-restore --configuration Release

      - name: Test
        run: dotnet test --no-build --verbosity normal --collect:"XPlat Code Coverage"

      - name: Publish
        run: dotnet publish -c Release -o ./publish

      - name: Upload artifact
        uses: actions/upload-artifact@v3
        with:
          name: dotnet-app
          path: ./publish

  docker-build:
    needs: build-and-test
    runs-on: ubuntu-latest

    steps:
      - uses: actions/checkout@v4

      - name: Build Docker image
        run: docker build -t myapp:${{ github.sha }} .

      - name: Push to ACR
        run: |
          echo ${{ secrets.ACR_PASSWORD }} | docker login ${{ secrets.ACR_LOGIN_SERVER }} -u ${{ secrets.ACR_USERNAME }} --password-stdin
          docker tag myapp:${{ github.sha }} ${{ secrets.ACR_LOGIN_SERVER }}/myapp:${{ github.sha }}
          docker push ${{ secrets.ACR_LOGIN_SERVER }}/myapp:${{ github.sha }}
```

### 10.3 Reusable Workflows

```yaml
# .github/workflows/reusable-deploy.yml
name: Reusable Deploy Workflow

on:
  workflow_call:
    inputs:
      environment:
        required: true
        type: string
      image-tag:
        required: true
        type: string
    secrets:
      DEPLOY_TOKEN:
        required: true

jobs:
  deploy:
    runs-on: ubuntu-latest
    environment: ${{ inputs.environment }}

    steps:
      - name: Deploy application
        run: |
          echo "Deploying to ${{ inputs.environment }}"
          echo "Image tag: ${{ inputs.image-tag }}"
```

```yaml
# .github/workflows/main.yml
name: Main Pipeline

on: [push]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - run: echo "Building..."

  deploy-staging:
    needs: build
    uses: ./.github/workflows/reusable-deploy.yml
    with:
      environment: staging
      image-tag: ${{ github.sha }}
    secrets:
      DEPLOY_TOKEN: ${{ secrets.STAGING_DEPLOY_TOKEN }}

  deploy-production:
    needs: deploy-staging
    uses: ./.github/workflows/reusable-deploy.yml
    with:
      environment: production
      image-tag: ${{ github.sha }}
    secrets:
      DEPLOY_TOKEN: ${{ secrets.PROD_DEPLOY_TOKEN }}
```

---

**[Continue in Part 2 with Azure DevOps, GitLab CI, Jenkins, and more...]**
