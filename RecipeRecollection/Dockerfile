#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN apt-get update && apt-get install --no-install-recommends -y python3.9 python3.9-dev python3-pip python3-wheel build-essential && \
   apt-get clean && rm -rf /var/lib/apt/lists/*
RUN pip3 install beautifulsoup4 requests

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RecipeRecollection/RecipeRecollection.csproj", "RecipeRecollection/"]
RUN dotnet restore "RecipeRecollection/RecipeRecollection.csproj"
COPY . .
WORKDIR "/src/RecipeRecollection"
RUN dotnet build "RecipeRecollection.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RecipeRecollection.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RecipeRecollection.dll"]