namespace Data.Repository.Interfaces
{
    public interface IDbContext
    {
        int SaveChanges();
        void Dispose();
    }
}