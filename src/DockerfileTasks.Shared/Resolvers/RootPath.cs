using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DockerfileTasks.DockerfileTasks.Shared.Resolvers
{
    internal static class RootPath
    {
        public static bool IsChildTo(this string path, string root)
        {
            var pathSegments = Path.GetFullPath(path).TrimTrailingSeparator().Split(Path.DirectorySeparatorChar);
            var rootSegments = Path.GetFullPath(root).TrimTrailingSeparator().Split(Path.DirectorySeparatorChar);

            var pathStack = new Stack<string>(pathSegments.Reverse());
            var rootStack = new Stack<string>(rootSegments.Reverse());

            while (pathStack.Count > 0 && rootStack.Count > 0)
            {
                if (pathStack.Pop() != rootStack.Pop())
                    return false;
            }

            return rootStack.Count == 0;
        }
        
        public static string? GetPathRelativeTo(this string path, string root)
        {
            if (!Path.IsPathRooted(root))
                return default;
            
            var pathSegments = Path.GetFullPath(path).TrimTrailingSeparator().Split(Path.DirectorySeparatorChar);
            var rootSegments = Path.GetFullPath(root).TrimTrailingSeparator().Split(Path.DirectorySeparatorChar);

            var pathStack = new Stack<string>(pathSegments.Reverse());
            var rootStack = new Stack<string>(rootSegments.Reverse());

            while (pathStack.Count > 0 && rootStack.Count > 0)
            {
                if (pathStack.Pop() != rootStack.Pop())
                    return default;
            }

            return rootStack.Count > 0 ? default : pathStack.Aggregate(".", Path.Combine);
        }
    }
}