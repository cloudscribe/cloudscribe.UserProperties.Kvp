namespace cloudscribe.Kvp.Storage.EFCore.Common
{
    public interface IKvpDbContextFactory
    {
        IKvpDbContext Create();
    }
}
