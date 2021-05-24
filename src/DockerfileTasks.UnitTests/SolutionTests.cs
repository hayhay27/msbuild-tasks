using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using DockerfileTasks.DockerfileTasks.Shared.Parsers;
using Xunit;

namespace DockerfileTasks.UnitTests
{
    public class SolutionTests
    {
        [Fact]
        public void Parse()
        {
            var root = Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString("N"));
            using var _ = new FakeDirectory()
                .CreateFile("SomeSolution.sln",
                    Fake.SolutionEntry(@"DockerfileTasks\DockerfileTasks.csproj"),
                    Fake.SolutionEntry(@"SomeProject\SomeProject.csproj"),
                    Fake.SolutionEntry(@"Test\Test.csproj"))
                .Build(root);
            
            
            var solution = Solution.Parse(Path.Combine(root, "SomeSolution.sln"));
            
            
            Assert.Equal(3, solution.Projects.Count);
            Assert.Collection(solution.Projects,
                p => Assert.Equal(Path.Combine(root, $@"DockerfileTasks{Path.DirectorySeparatorChar}DockerfileTasks.csproj"), p),
                p => Assert.Equal(Path.Combine(root, $@"SomeProject{Path.DirectorySeparatorChar}SomeProject.csproj"), p),
                p => Assert.Equal(Path.Combine(root, $@"Test{Path.DirectorySeparatorChar}Test.csproj"), p));
        }

        private static IEnumerable<string[]> TestData()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                yield return new [] {@".\src\SomeSolution.sln"};
            }
        }
    }
}