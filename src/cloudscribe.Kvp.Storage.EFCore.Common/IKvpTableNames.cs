namespace cloudscribe.Kvp.Storage.EFCore.Common
{
    public interface IKvpTableNames
    {
        string TablePrefix { get; }
        string KvpItemTableName { get; }
    }
}
