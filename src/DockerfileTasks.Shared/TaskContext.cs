using System.Text.RegularExpressions;
using DockerfileTasks.DockerfileTasks.Shared.Parsers;

namespace DockerfileTasks.DockerfileTasks.Shared
{
    internal class TaskContext
    {
        public string ProjectDirectory { get; set; } = default!;
        public string DockerfileName { get; set; } = default!;
        public string StartToken { get; set; } = default!;
        public string EndToken { get; set; } = default!;

        public Solution Solution { get; set; } = default!;
        public string DockerfileContext { get; set; } = default!;
        public string? ExcludeExpression { get; set; }
        public bool UseSolutionAsRootInContainer { get; set; }
        public bool DumpProperties { get; set; }

        public bool Exclude(string sourcePath)
        {
            if (ExcludeExpression == null)
                return false;

            var regex = new Regex(ExcludeExpression, RegexOptions.Compiled);
            return regex.IsMatch(sourcePath);
        }
    }
}