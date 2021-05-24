FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
#>>>COPY_CSPROJ>>>
# CurrentDirectory: /Users/hayhay/repos/msbuild-tasks/src/Test
# SolutionFile: /Users/hayhay/repos/msbuild-tasks/src/MSBuildTasks.sln
# Dockerfile: /Users/hayhay/repos/msbuild-tasks/src/Test/Dockerfile
# Context: /Users/hayhay/repos/msbuild-tasks
COPY ["./src/MSBuildTasks.sln", "./"]
COPY ["./src/DockerfileTasks/DockerfileTasks.csproj", "./src/"]
COPY ["./src/Test/Test.csproj", "./src/"]
COPY ["./src/DockerfileTasks.UnitTests/DockerfileTasks.UnitTests.csproj", "./src/"]
COPY ["./src/../build/_build.csproj", "./src/"]
COPY ["./src/nested/NestedTest/NestedTest.csproj", "./src/"]
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
