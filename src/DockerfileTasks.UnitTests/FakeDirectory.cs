using System;
using System.Collections.Generic;
using System.IO;

namespace DockerfileTasks.UnitTests
{
    public class FakeDirectory
    {
        private readonly List<Func<string, IDisposable>> _configurations = new();


        public FakeDirectory CreateDir(string name, Action<FakeDirectory>? nestedAction = null)
        {
            _configurations.Add(ctx =>
            {
                
                var path = Path.Combine(ctx, name);
                
                if (nestedAction == null)
                {
                    Directory.CreateDirectory(path);
                    return new Disposable(() => Directory.Delete(path));
                }
                
                var nestedDirectory = new FakeDirectory();
                nestedAction.Invoke(nestedDirectory);
                return nestedDirectory.Build(path);
            });
            return this;
        }
        
        public FakeDirectory CreateFile(string name, params string[] fileContent)
        {
            _configurations.Add(ctx =>
            {
                var path = Path.Combine(ctx, name);
                if (fileContent.Length == 0)
                    File.WriteAllText(path, "");
                else
                    File.WriteAllLines(path, fileContent);
                return new Disposable(() => File.Delete(path));
            });
            return this;
        }

        public IDisposable Build(string root)
        {
            Stack<IDisposable> disposables = new();
            Directory.CreateDirectory(root);
            foreach (var action in _configurations)
            {
                disposables.Push(action.Invoke(root));
            }
            return new Disposable(() =>
            {
                while (disposables.Count > 0)
                {
                    var d = disposables.Pop();
                    d.Dispose();
                }

                Directory.Delete(root);
            });
        }

        class Disposable : IDisposable
        {
            private readonly Action? _rollback;

            private Disposable() { }

            public Disposable(Action rollback) => _rollback = rollback;

            public void Dispose() => _rollback?.Invoke();

            public static IDisposable Empty { get; } = new Disposable();
        }
    }
}