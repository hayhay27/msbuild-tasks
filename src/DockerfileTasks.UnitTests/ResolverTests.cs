using System;
using System.IO;
using DockerfileTasks.DockerfileTasks.Shared.Resolvers;
using Xunit;

namespace DockerfileTasks.UnitTests
{
    public class ResolverTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("./src/SomeSolution.sln")]
        public void ResolveSolutionFile(string? solutionFile)
        {
            var root = Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString("N"));
            using var _ = new FakeDirectory()
                .CreateDir(".git")
                .CreateDir("src", nested => nested
                    .CreateFile("SomeSolution.sln", 
                        Fake.SolutionEntry(@"DockerfileTasks\DockerfileTasks.csproj"),
                        Fake.SolutionEntry(@"SomeProject\SomeProject.csproj"),
                        Fake.SolutionEntry(@"Test\Test.csproj"))
                    .CreateDir("SomeProject", project => project
                        .CreateFile("Dockerfile")
                        .CreateFile("SomeProject.csproj"))
                    .CreateDir("SomeProject2", project => project
                        .CreateFile("SomeProject.csproj")))
                .Build(root);
            
            
            var result = Resolver.TryResolveSolution(NullLogger.Instance, Path.Combine(root, "src", "SomeProject"), solutionFile, out var solution);
            
            
            Assert.True(result);
            Assert.True(File.Exists(solution!.Path));
        }
        
        [Fact]
        public void TooMuchSolutionFiles()
        {
            var root = Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString("N"));
            using var _ = new FakeDirectory()
                .CreateDir(".git")
                .CreateDir("src", nested => nested
                    .CreateFile("SomeSolution.sln")
                    .CreateFile("OtherSolution.sln")
                    .CreateDir("SomeProject", project => project
                        .CreateFile("Dockerfile")
                        .CreateFile("SomeProject.csproj"))
                    .CreateDir("SomeProject2", project => project
                        .CreateFile("SomeProject.csproj")))
                .Build(root);

            var result = Resolver.TryResolveSolution(NullLogger.Instance, Path.Combine(root, "src", "SomeProject"), Path.Combine(".", "src", "SomeSolution.sln"), out var _);
            
            Assert.False(result);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("../..")]
        [InlineData("../../")]
        [InlineData("../")]
        public void ResolveContext(string? context)
        {
            var root = Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString("N"));
            using var _ = new FakeDirectory()
                .CreateDir(".git")
                .CreateDir("src", nested => nested
                    .CreateFile("SomeSolution.sln", 
                        Fake.SolutionEntry(@"DockerfileTasks\DockerfileTasks.csproj"),
                        Fake.SolutionEntry(@"SomeProject\SomeProject.csproj"),
                        Fake.SolutionEntry(@"Test\Test.csproj"))
                    .CreateDir("SomeProject", project => project
                        .CreateFile("Dockerfile")
                        .CreateFile("SomeProject.csproj"))
                    .CreateDir("SomeProject2", project => project
                        .CreateFile("SomeProject.csproj")))
                .Build(root);
            
            var result = Resolver.TryResolveContextRoot(NullLogger.Instance, Path.Combine(root, "src", "SomeProject"), context, out var contextRoot);


            Assert.True(result);
        }
    }
}