using cloudscribe.Versioning;
using System;
using System.Reflection;

namespace cloudscribe.UserProperties.Kvp
{
    public class VersionProvider : IVersionProvider
    {
        private Assembly assembly = typeof(UserPropertyService).Assembly;

        public string Name
        {
            get { return assembly.GetName().Name; }

        }

        public Guid ApplicationId { get { return new Guid("df37d633-1093-479f-8dbb-01d2da334723"); } }

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
