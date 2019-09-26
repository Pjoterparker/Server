namespace PjoterParker.Core.Services
{
    public interface IDbContext
    {
        void BeginTransaction();

        void RollbackTransaction();

        void SaveChanges();
    }
}
