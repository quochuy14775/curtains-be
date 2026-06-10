using curtains_be.Data;

namespace curtains_be.Repository
{
    public class Repository<T> : RepositoryWithTypedId<T, int>, IRepository<T>
        where T : class
    {
        public Repository(AppDbContext context) : base(context)
        {
        }
    }
}