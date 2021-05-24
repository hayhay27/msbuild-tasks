using System.Collections.Generic;
using System.IO;
using System.Linq;
using DockerfileTasks.DockerfileTasks.Shared.Resolvers;
using DockerfileTasks.Logging;

namespace DockerfileTasks.DockerfileTasks.Shared.Parsers
{
    internal class Dockerfile
    {
        public static bool TryParse(ILogger logger, TaskContext ctx, out IReadOnlyCollection<string>? parsedContent)
        {
            var path = Path.Combine(ctx.ProjectDirectory, ctx.DockerfileName);
            if (!File.Exists(path))
            {
                logger.LogWarning("File \"{0}\" not found", path);
                parsedContent = default;
                return false;
            }

            var content = File.ReadAllLines(path);
            var startIndex = IndexOfToken(content, ctx.StartToken);
            if (startIndex == -1)
            {
                logger.LogWarning("Start token \"{0}\" not found", ctx.StartToken);
            }
            var endIndex = IndexOfToken(content, ctx.EndToken, startIndex + 1);
            if (endIndex == -1)
            {
                logger.LogWarning("End token \"{0}\" not found or placed before start token", ctx.EndToken);
            }
            var copyDirectives = ProjectCopyDirectives(ctx).ToArray();
            parsedContent = ParseInternal(content, copyDirectives, startIndex, endIndex).ToArray();
            return true;
        }

        public static void Save(string dockerfile, IReadOnlyCollection<string> content)
        {
            File.WriteAllLines(dockerfile, content);
        }

        private static int IndexOfToken(IReadOnlyList<string> content, string token, int start = 0)
        {
            for (var i = start; i < content.Count; i++)
            {
                if (content[i].Contains(token))
                    return i;
            }

            return -1;
        }

        private static IEnumerable<string> ParseInternal(IReadOnlyList<string> content, IReadOnlyCollection<string> copyDirectives, int startTokenIndex, int endTokenIndex)
        {
            for (var i = 0; i < content.Count; i++)
            {
                
                if (i < startTokenIndex || i > endTokenIndex)
                    yield return content[i];
                else
                {
                    yield return content[i];
                    foreach (var copyDirective in copyDirectives)
                    {
                        yield return copyDirective;
                    }
                    i = endTokenIndex;
                    yield return content[i];
                }
            }
        }

        private static IEnumerable<string> ProjectCopyDirectives(TaskContext ctx)
        {
            if (ctx.DumpProperties)
            {
                yield return $"# CurrentDirectory: {ctx.ProjectDirectory}";
                yield return $"# SolutionFile: {ctx.Solution.Path}";
                yield return $"# Dockerfile: {ctx.DockerfileName}";
                yield return $"# Context: {ctx.DockerfileContext}";
            }
            
            yield return $"COPY [\"{ctx.Solution.Path.GetPathRelativeTo(ctx.DockerfileContext)?.Replace('\\', '/')}\", \"./\"]";
            foreach (var project in ctx.Solution.Projects)
            {
                var source = project.GetPathRelativeTo(ctx.DockerfileContext)?.Replace('\\', '/');
                var destination = project.GetDirectory()?.GetPathRelativeTo(ctx.DockerfileContext)?.Replace('\\', '/');
                yield return $"COPY [\"{source}\", \"{destination}/\"]";
            }
        }
    }
}