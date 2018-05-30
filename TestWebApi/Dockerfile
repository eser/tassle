# Install .NET Core Runtime docker container
FROM microsoft/dotnet:aspnetcore-runtime

# Environment variables
ENV ASPNETCORE_ENVIRONMENT production

# Create app directory
WORKDIR /app

# Copy build output
COPY . .

# Export the port 80
EXPOSE 80

ENTRYPOINT ["dotnet", "Tassle.TestWebApi.dll"]
