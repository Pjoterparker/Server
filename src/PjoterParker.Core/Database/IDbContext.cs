namespace PjoterParker.Core.Database
{
    public interface IDbContext
    {
        void BeginTransaction();

        void RollbackTransaction();

        void SaveChanges();
    }
}