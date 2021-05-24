using System.IO;

namespace DockerfileTasks.DockerfileTasks.Shared
{
    public static class PathExtensions
    {
        internal static string TrimTrailingSeparator(this string path)
        {
            return path.Length == 1 ? path : path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }
    }
}