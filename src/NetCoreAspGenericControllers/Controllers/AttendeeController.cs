using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetCoreAspGenericControllers.Abstract;
using NetCoreAspGenericControllers.Model;
using NetCoreAspGenericControllers.ViewModels;
using System;

namespace NetCoreAspGenericControllers.Controllers
{
    [Route("api/[controller]")]
	public class AttendeeController : GenericCrudController<Attendee, AttendeeViewModel>
    {
		public AttendeeController(IConfigurationRoot configuration, IEntityBaseRepository<Attendee> repository)
			: base(configuration, repository)
		{
		}

		public override Attendee CreateEntity(AttendeeViewModel viewModel)
		{
			return default(Attendee);
		}

		public override void UpdateEntity(ref Attendee entity, AttendeeViewModel viewModel)
		{
			throw new NotImplementedException();
		}
	}
}

