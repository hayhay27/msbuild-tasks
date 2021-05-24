using System.IO;
using DockerfileTasks.DockerfileTasks.Shared.Parsers;
using DockerfileTasks.DockerfileTasks.Shared.Resolvers;

namespace DockerfileTasks.DockerfileTasks.Shared
{
    internal static class Extensions
    {
        internal static string TrimTrailingSeparator(this string path)
        {
            return path.Length == 1 ? path : path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }
        
        public static string? GetDirectory(this Project project)
        {
            return Path.GetDirectoryName(project.Path);
        }
        
        public static string? GetPathRelativeTo(this Project project, string root)
        {
            return project.Path.GetPathRelativeTo(root);
        }
        
        public static string? GetDirectory(this Solution project)
        {
            return Path.GetDirectoryName(project.Path);
        }
        
        public static string? GetPathRelativeTo(this Solution project, string root)
        {
            return project.Path.GetPathRelativeTo(root);
        }
    }
}