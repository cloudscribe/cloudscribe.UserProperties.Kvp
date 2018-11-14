using cloudscribe.Versioning;
using System;
using System.Reflection;

namespace cloudscribe.Kvp.Storage.EFCore.MSSQL
{
    public class VersionProvider : IVersionProvider
    {
        private Assembly assembly = typeof(KvpDbContext).Assembly;

        public string Name
        {
            get { return assembly.GetName().Name; }

        }

        public Guid ApplicationId { get { return new Guid("7a70dd4d-edd5-4a17-a638-9c6c3e6c6736"); } }

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
