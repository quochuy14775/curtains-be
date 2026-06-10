namespace curtains_be.Repository
{
    public interface IRepository<T> : IRepositoryWithTypedId<T, int> where T : class
    {
    }
}