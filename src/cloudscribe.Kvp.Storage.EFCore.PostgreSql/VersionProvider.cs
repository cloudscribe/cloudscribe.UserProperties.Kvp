using cloudscribe.Versioning;
using System;
using System.Reflection;

namespace cloudscribe.Kvp.Storage.EFCore.PostgreSql
{
    public class VersionProvider : IVersionProvider
    {
        private Assembly assembly = typeof(KvpDbContext).Assembly;

        public string Name
        {
            get { return assembly.GetName().Name; }

        }

        public Guid ApplicationId { get { return new Guid("641d3a05-708f-4f0f-a420-08bdeac1b30c"); } }

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
