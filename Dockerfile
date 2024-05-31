# docker build -t simpleapi:latest .  
# docker run -p 5000:5000 simpleapi:latest


# Use the official .NET image as a build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build



# Set the working directory
WORKDIR /src

# Copy the project files
COPY ./SimpleApi.Api/SimpleApi.API.csproj SimpleApi.Api/
COPY ./SimpleApi.Domain/SimpleApi.Domain.csproj SimpleApi.Domain/
COPY ./SimpleApi.Services/SimpleApi.Services.csproj SimpleApi.Services/

# Restore the dependencies
RUN dotnet restore SimpleApi.Api/SimpleApi.API.csproj

# Copy the rest of the files
COPY . .

# RUN printf "Contents of /src folder:\n" && ls -al /src
# RUN dotnet build SimpleApi.API/SimpleApi.API.csproj -c Release -o /app/build

# Build the project
RUN dotnet publish SimpleApi.Api/SimpleApi.API.csproj -c Release -o /app/publish --verbosity quiet 

# Use the official .NET runtime image as a runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Install curl
RUN apt-get update && apt-get install -y curl


# Set the working directory
WORKDIR /app

# Copy the build output
COPY --from=build /app/publish .

# Expose ports
EXPOSE 5000
# EXPOSE 5001

# Set the entry point
ENTRYPOINT ["dotnet", "SimpleApi.API.dll"]