namespace NetCoreAspGenericControllers.Model
{
    public class Attendee : EntityBase
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }
    }
}
