using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetCoreAspGenericControllers.Abstract;
using NetCoreAspGenericControllers.Model;
using NetCoreAspGenericControllers.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreAspGenericControllers.Controllers
{
	[Route("api/[controller]")]
	public class UserController : GenericCrudController<User, UserViewModel>
	{
		private readonly IEntityBaseRepository<User> _userRepository;
		private readonly IEntityBaseRepository<Attendee> _attendeeRepository;
		private readonly IEntityBaseRepository<Schedule> _scheduleRepository;

		public UserController(
			IConfigurationRoot configuration,
			IEntityBaseRepository<User> userRepository,
			IEntityBaseRepository<Attendee> attendeeRepository,
			IEntityBaseRepository<Schedule> scheduleRepository
			)
			: base(configuration, userRepository)
		{
			_userRepository = userRepository;
			_attendeeRepository = attendeeRepository;
			_scheduleRepository = scheduleRepository;
		}

		// Override base GetEntities method
		public override IEnumerable<User> GetEntities(int currentPage, int currentPageSize)
		{
			IEnumerable<User> entities = _userRepository
				.AllIncluding(u => u.SchedulesCreated)
				.OrderBy(u => u.Id)
				.Skip((currentPage - 1) * currentPageSize)
				.Take(currentPageSize)
				.ToList();

			return entities;
		}

		// Override base GetEntity method
		public override User GetEntity(int id)
		{
			User entity = _userRepository
				.GetSingle(
					u => u.Id == id,
					u => u.SchedulesCreated
				);

			return entity;
		}

		// Override base GetEntity method : Must Be Override
		public override User CreateEntity(UserViewModel viewModel)
		{
			User newEntity = new User
			{
				Name = viewModel.Name,
				Profession = viewModel.Profession,
				Avatar = viewModel.Avatar
			};

			return newEntity;
		}

		// Override base UpdateEntity method
		public override void UpdateEntity(ref User entity, UserViewModel viewModel)
		{
			entity.Name = viewModel.Name;
			entity.Profession = viewModel.Profession;
			entity.Avatar = viewModel.Avatar;
		}

		// Override base DeleteEntity method
		public override void DeleteEntity(int id)
		{
			IEnumerable<Attendee> _attendees = _attendeeRepository.FindBy(a => a.UserId == id);
			IEnumerable<Schedule> _schedules = _scheduleRepository.FindBy(s => s.CreatorId == id);

			foreach (var attendee in _attendees)
			{
				_attendeeRepository.Delete(attendee);
			}

			foreach (var schedule in _schedules)
			{
				_attendeeRepository.DeleteWhere(a => a.ScheduleId == schedule.Id);
				_scheduleRepository.Delete(schedule);
			}
		}

		//Custom Controller Method : Extend base Generic Controller
		[HttpGet("{id}/schedules", Name = "GetUserSchedules")]
		public IActionResult GetSchedules(int id)
		{
			IEnumerable<Schedule> userSchedules = _scheduleRepository.FindBy(s => s.CreatorId == id);

			if (userSchedules != null)
			{
				IEnumerable<ScheduleViewModel> userSchedulesViewModel = Mapper.Map<IEnumerable<Schedule>, IEnumerable<ScheduleViewModel>>(userSchedules);
				return new OkObjectResult(userSchedulesViewModel);
			}
			else
			{
				return NotFound();
			}
		}
	}
}
