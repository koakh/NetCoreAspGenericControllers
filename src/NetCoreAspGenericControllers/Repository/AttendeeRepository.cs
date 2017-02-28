using NetCoreAspGenericControllers.Abstract;
using NetCoreAspGenericControllers.Data;
using NetCoreAspGenericControllers.Model;

namespace NetCoreAspGenericControllers.Repository
{
    public class AttendeeRepository : EntityBaseRepository<Attendee>, IAttendeeRepository
    {
        public AttendeeRepository() { }

        public AttendeeRepository(DomainContext context)
            : base(context)
        { }
    }
}
