using NetCoreAspGenericControllers.Abstract;
using NetCoreAspGenericControllers.Data;
using NetCoreAspGenericControllers.Model;

namespace NetCoreAspGenericControllers.Repository
{
    public class ScheduleRepository : EntityBaseRepository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository() { }

        public ScheduleRepository(DomainContext context)
            : base(context)
        { }
    }
}
