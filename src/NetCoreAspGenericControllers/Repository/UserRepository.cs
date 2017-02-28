using NetCoreAspGenericControllers.Abstract;
using NetCoreAspGenericControllers.Data;
using NetCoreAspGenericControllers.Model;

namespace NetCoreAspGenericControllers.Repository
{
    public class UserRepository : EntityBaseRepository<User>, IUserRepository
    {
        public UserRepository() { }

        public UserRepository(DomainContext context)
            : base(context)
        { }
    }
}
