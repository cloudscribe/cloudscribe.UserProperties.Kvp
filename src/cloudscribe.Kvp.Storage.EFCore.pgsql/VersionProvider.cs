using cloudscribe.Versioning;
using System;
using System.Reflection;

namespace cloudscribe.Kvp.Storage.EFCore.pgsql
{
    public class VersionProvider : IVersionProvider
    {
        private Assembly assembly = typeof(KvpDbContext).Assembly;

        public string Name
        {
            get { return assembly.GetName().Name; }

        }

        public Guid ApplicationId { get { return new Guid("7d839dfa-2967-4952-b23a-11b88508d49b"); } }

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
