using Microsoft.AspNetCore.OData.Routing.Controllers;
using curtains_be.Repository;

namespace curtains_be.Controllers;

public class BaseController<TEntity> : ODataController where TEntity : class
{
    protected readonly IRepository<TEntity> _baseRepository;

    public BaseController(IRepository<TEntity> baseRepository)
    {
        _baseRepository = baseRepository;
    }
}
