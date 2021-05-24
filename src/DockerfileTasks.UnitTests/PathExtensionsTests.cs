using System.Collections.Generic;
using System.Runtime.InteropServices;
using DockerfileTasks.DockerfileTasks.Shared.Resolvers;
using Xunit;

namespace DockerfileTasks.UnitTests
{
    public class PathExtensionsTests
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public void GetRelativePathTest(string path, string root, string? relativePath)
        {
            var result = path.GetPathRelativeTo(root);
            Assert.Equal(relativePath, result);
        }
        
        [Theory]
        [MemberData(nameof(TestData))]
        public void IsChildToTest(string path, string root, string? relativePath)
        {
            var result = path.IsChildTo(root);
            Assert.Equal(relativePath != null, result);
        }

        public static IEnumerable<string?[]> TestData()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                yield return new[] {@"C:\var\log\app", @"C:\var\log", @".\app"};
                yield return new[] {@"C:\var\log\app\", @"C:\var\log", @".\app"};
                yield return new[] {@"C:\var\log\app", @"C:\var\log\", @".\app"};
                yield return new[] {@"C:\var\log\app\", @"C:\var\log\", @".\app"};

                yield return new[] {@"C:\var\log\app", @"C:\var\log\app", @"."};
                yield return new[] {@"C:\var\log\app\", @"C:\var\log\app", @"."};
                yield return new[] {@"C:\var\log\app", @"C:\var\log\app\", @"."};
                yield return new[] {@"C:\var\log\app\", @"C:\var\log\app\", @"."};
                
                yield return new[] {@"C:\var\log\app\", @"C:\var\log\app", @"."};
                yield return new[] {@"C:\var\log\app", @"C:\var\log\app\", @"."};
                yield return new[] {@"C:\var\log\app\", @"C:\var\log\app\", @"."};

                // not root
                yield return new[] {@"C:\etc\data\app\", @"C:\var\log\", null};
                yield return new[] {@"C:\var\", @"C:\var\log\", null};
                yield return new[] {@"C:\var\log\app", @"..\", null};
                yield return new[] {@"C:\var\log\app", @"..\..\", null};

            }
            else
            {
                yield return new[] {"/var/log/app", "/var/log", "./app"};
                yield return new[] {"/var/log/app/", "/var/log", "./app"};
                yield return new[] {"/var/log/app", "/var/log/", "./app"};
                yield return new[] {"/var/log/app/", "/var/log/", "./app"};

                yield return new[] {"/var/log/app", "/var/log/app", "."};
                yield return new[] {"/var/log/app/", "/var/log/app", "."};
                yield return new[] {"/var/log/app", "/var/log/app/", "."};
                yield return new[] {"/var/log/app/", "/var/log/app/", "."};

                // not root
                yield return new[] {"/etc/data/app/", "/var/log/", null};
            }
        }
    }
}