FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NestedTest/NestedTest.csproj", "NestedTest/"]
RUN dotnet restore "NestedTest/NestedTest.csproj"
COPY . .
WORKDIR "/src/NestedTest"
RUN dotnet build "NestedTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NestedTest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NestedTest.dll"]
