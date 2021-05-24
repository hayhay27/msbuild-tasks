FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
#>>>COPY_CSPROJ>>>
# CurrentDirectory: F:\sibur\dev\repos\DockerfileTasks\Test
# SolutionFile: F:\sibur\dev\repos\DockerfileTasks\MSBuildTasks.sln
# Dockerfile: F:\sibur\dev\repos\DockerfileTasks\Test\Dockerfile
# Context: F:\sibur\dev\repos\DockerfileTasks
COPY ["./MSBuildTasks.sln", "./"]
COPY ["./DockerfileTasks/DockerfileTasks.csproj", "./DockerfileTasks/"]
COPY ["./Test/Test.csproj", "./Test/"]
COPY ["./DockerfileTasks.UnitTests/DockerfileTasks.UnitTests.csproj", "./DockerfileTasks.UnitTests/"]
#<<<COPY_CSPROJ<<<
RUN dotnet restore "Test/Test.csproj"
COPY . .
WORKDIR "/src/Test"
RUN dotnet build "Test.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Test.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Test.dll"]
