# Use the .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY ["pentasharp.csproj", "./"]
RUN dotnet restore "pentasharp.csproj"

# Copy the rest of the files and build the app
COPY . .
RUN dotnet publish "pentasharp.csproj" -c Release -o /app/publish

# Set the base image to the .NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port
EXPOSE 80
ENTRYPOINT ["dotnet", "pentasharp.dll"]
