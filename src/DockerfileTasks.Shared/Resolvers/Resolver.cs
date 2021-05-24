using System.IO;
using System.Linq;
using DockerfileTasks.DockerfileTasks.Shared.Parsers;
using DockerfileTasks.Logging;

namespace DockerfileTasks.DockerfileTasks.Shared.Resolvers
{
    internal class Resolver
    {
        public static bool TryResolveSolution(ILogger logger, string projectDirectory, string? solutionFile, out Solution? solution)
        {
            if (solutionFile != null)
            {
                var path = Path.Combine(projectDirectory, solutionFile);
                if (File.Exists(path))
                {
                    solution = Solution.Parse(path);
                    return true;
                }
            }
            
            var levels = 2;
            var directory = projectDirectory;
            while (levels >= 0)
            {
                var files = Directory.EnumerateFiles(directory!, "*.sln").ToList();
                switch (files)
                {
                    case {Count: 0}:
                        directory = Path.GetDirectoryName(directory);
                        if (directory == null) break;
                        levels -= 1;
                        continue;
                    case {Count: 1}:
                        solution = Solution.Parse(files[0]);
                        return true;
                    default:
                        logger.LogWarning("Specify with property \"SolutionFile\" which solution file to use because folder \"{0}\" contains more than one solution file", directory!);
                        solution = default;
                        return false;
                }
            }
            
            logger.LogWarning("Solution file not found");
            solution = default;
            return false;
        }

        public static bool TryResolveContextRoot(ILogger logger, string projectDirectory, string? dockerfileContext, out string? contextRoot)
        {
            if (dockerfileContext == null)
                return TryResolveContextRootByRepository(logger, projectDirectory, out contextRoot);
            
            contextRoot = Path.GetFullPath(Path.Combine(projectDirectory, dockerfileContext));
            return Directory.Exists(contextRoot);
        }

        private static bool TryResolveContextRootByRepository(ILogger logger, string projectDirectory, out string? contextRoot)
        {
            var levels = 4;
            var directory = projectDirectory;
            while (levels >= 0)
            {
                var git = Path.Combine(directory!, ".git");
                if (Directory.Exists(git))
                {
                    contextRoot = directory;
                    return true;
                }

                directory = Path.GetDirectoryName(directory);
                levels -= 1;
            }

            logger.LogWarning("Cannot resolve context root by repository");
            contextRoot = default;
            return false;
        }
    }
}