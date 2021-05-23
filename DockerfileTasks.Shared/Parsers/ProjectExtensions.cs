using System.IO;
using DockerfileTasks.DockerfileTasks.Shared.Resolvers;

namespace DockerfileTasks.DockerfileTasks.Shared.Parsers
{
    internal static class ProjectExtensions
    {
        public static string? GetDirectory(this Project project)
        {
            return Path.GetDirectoryName(project.Path);
        }
        
        public static string? GetPathRelativeTo(this Project project, string root)
        {
            return project.Path.GetPathRelativeTo(root);
        }
    }
}