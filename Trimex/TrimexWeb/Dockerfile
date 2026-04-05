# Stage 1: Build
FROM node:20-alpine AS build

WORKDIR /app

# Copy package.json and package-lock.json if it exists
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy source files
COPY . .

# Build the application
RUN npm run build

# Stage 2: Serve with an npm server (serve)
FROM node:20-alpine

WORKDIR /app

# Install 'serve' package globally to provide the npm server
RUN npm install -g serve

# Copy built files from the build stage
COPY --from=build /app/dist /app/dist

# Expose port 3000
EXPOSE 3000

# Start the npm server
# -s flag for single page application (SPA)
CMD ["serve", "-s", "dist", "-l", "3000"]
