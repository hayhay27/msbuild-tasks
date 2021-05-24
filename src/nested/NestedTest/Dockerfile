FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
#>>>COPY_CSPROJ>>>
# CurrentDirectory: /Users/hayhay/repos/msbuild-tasks/src/nested/NestedTest
# Dockerfile: /Users/hayhay/repos/msbuild-tasks/src/nested/NestedTest/Dockerfile
# Context: /Users/hayhay/repos/msbuild-tasks
# SolutionFile: /Users/hayhay/repos/msbuild-tasks/src/MSBuildTasks.sln
#   Project: /Users/hayhay/repos/msbuild-tasks/src/DockerfileTasks/DockerfileTasks.csproj
#   Project: /Users/hayhay/repos/msbuild-tasks/src/Test/Test.csproj
#   Project: /Users/hayhay/repos/msbuild-tasks/src/DockerfileTasks.UnitTests/DockerfileTasks.UnitTests.csproj
#   Project: /Users/hayhay/repos/msbuild-tasks/build/_build.csproj
#   Project: /Users/hayhay/repos/msbuild-tasks/src/nested/NestedTest/NestedTest.csproj
COPY ["./src/MSBuildTasks.sln", "./"]
COPY ["./src/DockerfileTasks/DockerfileTasks.csproj", "./DockerfileTasks/"]
COPY ["./src/Test/Test.csproj", "./Test/"]
COPY ["./src/DockerfileTasks.UnitTests/DockerfileTasks.UnitTests.csproj", "./DockerfileTasks.UnitTests/"]
COPY ["./src/nested/NestedTest/NestedTest.csproj", "./nested/NestedTest/"]
#<<<COPY_CSPROJ<<<
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
