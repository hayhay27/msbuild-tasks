using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DockerfileTasks.DockerfileTasks.Shared.Parsers
{
    internal class Solution
    {
        public Solution(string path, IReadOnlyCollection<Project> projects)
        {
            Path = path;
            Projects = projects;
        }

        public string Path { get; }
        public IReadOnlyCollection<Project> Projects { get; }

        public static Solution Parse(string solutionFile)
        {
            var projects = GetProjects(solutionFile);
            return new Solution(solutionFile, projects);
        }

        private static IReadOnlyCollection<Project> GetProjects(string solutionFile)
        {
            var solutionDir = System.IO.Path.GetDirectoryName(solutionFile)!;
            return File.ReadAllLines(solutionFile)
                .Where(x => x.Contains(".csproj"))
                .Select(x =>
                {
                    var col = x.Split('"');
                    return new Project(System.IO.Path.GetFullPath(System.IO.Path.Combine(solutionDir, col[5])));
                })
                .ToArray();
        }
    }
    
    internal class Project
    {
        public Project(string path) => Path = path;

        public string Path { get; }
        public static implicit operator string(Project project) => project.Path;
    }
}