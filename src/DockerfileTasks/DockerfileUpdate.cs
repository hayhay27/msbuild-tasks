using System;
using System.IO;
using DockerfileTasks.DockerfileTasks.Shared;
using DockerfileTasks.DockerfileTasks.Shared.Parsers;
using DockerfileTasks.DockerfileTasks.Shared.Resolvers;
using DockerfileTasks.Logging;
using Microsoft.Build.Utilities;

namespace DockerfileTasks
{
    public class DockerfileUpdate : Task
    {
        public override bool Execute()
        {
            var logger = new Logger(Log);
            try
            {
                
                logger.LogLow("DockerfileName: \"{0}\"", DockerfileName);
                logger.LogLow("StartToken: \"{0}\"", StartToken);
                logger.LogLow("EndToken: \"{0}\"", EndToken);
                logger.LogLow("DockerfileContext: \"{0}\"", DockerfileContext ?? "null");
                logger.LogLow("SolutionFile: \"{0}\"", SolutionFile ?? "null");

                if (!Resolver.TryResolveSolution(logger, Directory.GetCurrentDirectory(), SolutionFile, out var solution) ||
                    !Resolver.TryResolveContextRoot(logger, Directory.GetCurrentDirectory(), DockerfileContext, out var contextRoot))
                {
                    return true;
                }
                
                var ctx = new TaskContext
                {
                    ProjectDirectory = Directory.GetCurrentDirectory(),
                    DockerfileName = Path.Combine(Directory.GetCurrentDirectory(), DockerfileName),
                    StartToken = StartToken,
                    EndToken = EndToken,
                    Solution = solution!,
                    DockerfileContext = contextRoot!,
                    ExcludeExpression = ExcludeExpression,
                    UseSolutionAsRootInContainer = UseSolutionAsRootInContainer,
                    DumpProperties = DumpProperties
                };

                if (Dockerfile.TryParse(logger, ctx, out var dockerfile))
                {
                    Dockerfile.Save(ctx.DockerfileName, dockerfile!);
                }
                return true;
            }
            catch (Exception e)
            {
                logger.LogWarning(e);
                return true;
            }
        }

        public string DockerfileName { get; set; } = "Dockerfile";
        public string StartToken { get; set; } = "#>>>COPY_CSPROJ>>>";
        public string EndToken { get; set; } = "#<<<COPY_CSPROJ<<<";
        public string? SolutionFile { get; set; } = default!;
        public string? DockerfileContext { get; set; } = default!;
        public string ExcludeExpression { get; set; } = default!;
        public bool UseSolutionAsRootInContainer { get; set; } = true;
        
        public bool DumpProperties { get; set; }
    }
}