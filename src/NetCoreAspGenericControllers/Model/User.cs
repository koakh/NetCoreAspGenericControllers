using NetCoreAspGenericControllers.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAspGenericControllers.Model
{
    public class User : EntityBase
    {
        public User()
        {
            SchedulesCreated = new List<Schedule>();
            SchedulesAttended = new List<Attendee>();
        }
        
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Profession { get; set; }
        public ICollection<Schedule> SchedulesCreated { get; set; }
        public ICollection<Attendee> SchedulesAttended { get; set; }
    }
}
