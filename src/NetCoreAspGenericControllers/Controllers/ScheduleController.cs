using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetCoreAspGenericControllers.Abstract;
using NetCoreAspGenericControllers.Model;
using NetCoreAspGenericControllers.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreAspGenericControllers.Controllers
{
	[Route("api/[controller]")]
	public class ScheduleController : GenericCrudController<Schedule, ScheduleViewModel>
	{
		private readonly IEntityBaseRepository<Schedule> _scheduleRepository;
		private readonly IEntityBaseRepository<Attendee> _attendeeRepository;
		private readonly IEntityBaseRepository<User> _userRepository;

		public ScheduleController(
			IConfigurationRoot configuration,
			IEntityBaseRepository<Schedule> scheduleRepository,
			IEntityBaseRepository<Attendee> attendeeRepository,
			IEntityBaseRepository<User> userRepository
			)
			: base(configuration, scheduleRepository)
		{
			_scheduleRepository = scheduleRepository;
			_attendeeRepository = attendeeRepository;
			_userRepository = userRepository;
		}

		// Override base GetEntities method
		public override IEnumerable<Schedule> GetEntities(int currentPage, int currentPageSize)
		{
			IEnumerable<Schedule> entities = _scheduleRepository
				.AllIncluding(s => s.Creator, s => s.Attendees)
				.OrderBy(s => s.Id)
				.Skip((currentPage - 1) * currentPageSize)
				.Take(currentPageSize)
				.ToList();

			return entities;
		}

		// Override base GetEntity method
		public override Schedule GetEntity(int id)
		{
			Schedule entity = _scheduleRepository
				.GetSingle(
					s => s.Id == id,
					s => s.Creator, s => s.Attendees
				);

			return entity;
		}

		// Override base CreateEntity method
		public override Schedule CreateEntity(ScheduleViewModel viewModel)
		{
			Schedule _newEntity = Mapper.Map<ScheduleViewModel, Schedule>(viewModel);
			_newEntity.DateCreated = DateTime.Now;

			return _newEntity;
		}

		// Override base UpdateEntity method
		public override void UpdateEntity(ref Schedule entity, ScheduleViewModel viewModel)
		{
			int id = entity.Id;
			entity.Title = viewModel.Title;
			entity.Location = viewModel.Location;
			entity.Description = viewModel.Description;
			entity.Status = (ScheduleStatus)Enum.Parse(typeof(ScheduleStatus), viewModel.Status);
			entity.Type = (ScheduleType)Enum.Parse(typeof(ScheduleType), viewModel.Type);
			entity.TimeStart = viewModel.TimeStart;
			entity.TimeEnd = viewModel.TimeEnd;

			// Remove current attendees
			_attendeeRepository.DeleteWhere(a => a.ScheduleId == id);

			foreach (var userId in viewModel.Attendees)
			{
				entity.Attendees.Add(new Attendee { ScheduleId = id, UserId = userId });
			}
		}

		// Override base DeleteEntity method
		public override void DeleteEntity(int id) {
			_attendeeRepository.DeleteWhere(a => a.ScheduleId == id);
		}

		//Custom Controller Method : Extend base Generic Controller
		[HttpDelete("{id}/removeattendee/{attendee}")]
		public IActionResult Delete(int id, int attendee)
		{
			Schedule _entityDb = _scheduleRepository.GetSingle(id);

			if (_entityDb == null)
			{
				return new NotFoundResult();
			}
			else
			{
				_attendeeRepository.DeleteWhere(a => a.ScheduleId == id && a.UserId == attendee);

				_attendeeRepository.Commit();

				return new NoContentResult();
			}
		}
	}
}
