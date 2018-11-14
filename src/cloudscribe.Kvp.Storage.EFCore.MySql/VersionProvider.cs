using cloudscribe.Versioning;
using System;
using System.Reflection;

namespace cloudscribe.Kvp.Storage.EFCore.MySql
{
    public class VersionProvider : IVersionProvider
    {
        private Assembly assembly = typeof(KvpDbContext).Assembly;

        public string Name
        {
            get { return assembly.GetName().Name; }

        }

        public Guid ApplicationId { get { return new Guid("3fb0b8d4-38a8-457b-af1c-d5bb32141b54"); } }

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
