using NetCoreAspGenericControllers.Model;

namespace NetCoreAspGenericControllers.Abstract
{
    public interface IScheduleRepository : IEntityBaseRepository<Schedule> { }
    public interface IUserRepository : IEntityBaseRepository<User> { }
    public interface IAttendeeRepository : IEntityBaseRepository<Attendee> { }
}
