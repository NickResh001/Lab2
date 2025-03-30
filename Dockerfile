# Используем .NET SDK для сборки и публикации
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . .

WORKDIR /app/Lab2
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Указываем контейнеру, что он будет работать на порту 8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Lab2.dll"]