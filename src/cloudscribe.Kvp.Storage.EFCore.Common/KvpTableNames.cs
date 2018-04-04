namespace cloudscribe.Kvp.Storage.EFCore.Common
{
    public class KvpTableNames : IKvpTableNames
    {
        public string TablePrefix { get; set; } = "cs_";
        public string KvpItemTableName { get; set; } = "KvpItem";
    }
}
