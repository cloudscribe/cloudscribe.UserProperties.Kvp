using cloudscribe.Versioning;
using System;
using System.Reflection;

namespace cloudscribe.Kvp.Storage.NoDb
{
    public class VersionProvider : IVersionProvider
    {
        private Assembly assembly = typeof(KvpItemCommands).Assembly;

        public string Name
        {
            get { return assembly.GetName().Name; }

        }

        public Guid ApplicationId { get { return new Guid("b4742d9c-a968-48e6-84a1-5c1cc386153f"); } }

        public Version CurrentVersion
        {

            get
            {

                var version = new Version(2, 0, 0, 0);
                var versionString = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                if (!string.IsNullOrWhiteSpace(versionString))
                {
                    Version.TryParse(versionString, out version);
                }

                return version;
            }
        }
    }
}
