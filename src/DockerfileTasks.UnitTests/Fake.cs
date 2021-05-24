namespace DockerfileTasks.UnitTests
{
    public static class Fake
    {
        public static string SolutionEntry(string projectPath)
        {
            return $"Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"bla-bla\", \"{projectPath}\", \"{{19D016E5-E332-4022-B800-8D12F13D8BAB}}\"";
        }
    }
}