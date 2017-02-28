using Microsoft.AspNetCore.Mvc;
using NetCoreAspGenericControllers.Abstract;
using System.Linq;
using System;
using System.Collections.Generic;
using NetCoreAspGenericControllers.Core;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace NetCoreAspGenericControllers.Controllers
{
	[Route("generic-crud-controller")]
	public abstract class GenericCrudController<TEntity, TViewModel> : Controller, IGenericCrudController<TEntity, TViewModel>
		where TEntity : class, IEntityBase, new()
		where TViewModel : class, IViewModel, new()
	{
		private readonly IConfigurationRoot _configuration;
		private readonly IEntityBaseRepository<TEntity> _repository;
		private string _apiEndpoint = string.Empty;
		private int _page = 1;
		private int _pageSize = 10;

		public GenericCrudController(IConfigurationRoot configuration, IEntityBaseRepository<TEntity> repository)
		{
			_configuration = configuration;
			_repository = repository;
			_apiEndpoint = _configuration["Api:EndPoint"];
			_pageSize = Convert.ToInt16(_configuration["Api:PageSize"]);
		}

		[HttpGet]
		public IActionResult Get()
		{
			var pagination = Request.Headers["pagination"];

			if (!string.IsNullOrEmpty(pagination))
			{
				string[] vals = pagination.ToString().Split(',');
				int.TryParse(vals[0], out _page);
				int.TryParse(vals[1], out _pageSize);
			}

			int currentPage = _page;
			int currentPageSize = _pageSize;
			var totalEntities = _repository.Count();
			var totalPages = (int)Math.Ceiling((double)totalEntities / _pageSize);

			// Call GENERIC Crud virtual method, can be overriden by subclasses if needed extra configuration
			IEnumerable<TEntity> entities = GetEntities(currentPage, currentPageSize);

			// Map TEntity to TViewModel with AutoMapper
			IEnumerable<TViewModel> usersVM = AutoMapper.Mapper.Map<IEnumerable<TEntity>, IEnumerable<TViewModel>>(entities);

			// add Pagiantion to Response
			Response.AddPagination(_page, _pageSize, totalEntities, totalPages);

			return new OkObjectResult(usersVM);
		}

		[HttpGet("{id:int}")]
		public IActionResult Get(int id)
		{
			// Call GENERIC Crud virtual method, can be overriden by subclasses if needed extra configuration
			TEntity _entity = GetEntity(id);

			if (_entity != null)
			{
				TViewModel _userVM = AutoMapper.Mapper.Map<TEntity, TViewModel>(_entity);
				return new OkObjectResult(_userVM);
			}
			else
			{
				return NotFound();
			}
		}

		[HttpPost]
		public IActionResult Create([FromBody] TViewModel viewModel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			// Call GENERIC Crud virtual method, can be overriden by subclasses if needed extra configuration
			TEntity newEntity = CreateEntity(viewModel);

			//Add Entity to Repository
			_repository.Add(newEntity);
			//Repository Commit
			_repository.Commit();

			//AutoMapper Map ViewModel
			viewModel = AutoMapper.Mapper.Map<TEntity, TViewModel>(newEntity);

			//CreatedAtRouteResult result = CreatedAtRoute("GetUser", new { controller = "Users", id = viewModel.Id }, viewModel);
			ObjectResult result = new ObjectResult(viewModel);
			result.StatusCode = Convert.ToInt16(HttpStatusCode.Created);
			//Add Headers to Response
			Response.Headers.Add("Location", string.Format("{0}/{1}/{2}", _apiEndpoint, GetType().Name.ToLower().Replace("controller", string.Empty), viewModel.Id));

			return result;
		}

		[HttpPut("{id:int}")]
		public IActionResult Put(int id, [FromBody] TViewModel viewModel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			TEntity _entityDb = _repository.GetSingle(id);

			if (_entityDb == null)
			{
				return NotFound();
			}
			else
			{
				//Call GENERIC Crud abstract method, MUST be overriden by subclass
				UpdateEntity(ref _entityDb, viewModel);
				//Repository Commit
				_repository.Commit();
			}

			//AutoMapper Map ViewModel
			viewModel = AutoMapper.Mapper.Map<TEntity, TViewModel>(_entityDb);

			return new NoContentResult();
		}

		[HttpDelete("{id:int}")]
		public IActionResult Delete(int id)
		{
			TEntity _entityDb = _repository.GetSingle(id);

			if (_entityDb == null)
			{
				return new NotFoundResult();
			}
			else
			{
				// Call Subclass DeleteEntity
				DeleteEntity(id);

				// Always Delete Generic Entity after custom DeleteEntity subclass method, if implemented
				_repository.Delete(_entityDb);
				//Repository Commit
				_repository.Commit();

				return new NoContentResult();
			}
		}

		//Virtual method to be overrided by clients, if required extra configuration
		[ApiExplorerSettings(IgnoreApi = true)]
		public virtual IEnumerable<TEntity> GetEntities(int currentPage, int currentPageSize)
		{
			return _repository
				.GetAll()
				.OrderBy(u => u.Id)
				.Skip((currentPage - 1) * currentPageSize)
				.Take(currentPageSize)
				.ToList();
		}

		//Virtual method to be overrided by clients, if required extra configuration
		[ApiExplorerSettings(IgnoreApi = true)]
		public virtual TEntity GetEntity(int id)
		{
			return _repository.GetSingle(u => u.Id == id);
		}

		// Abstract Method : Must be implemented by Subclass
		[ApiExplorerSettings(IgnoreApi = true)]
		public abstract TEntity CreateEntity(TViewModel viewModel);

		// Abstract Method : Must be implemented by Subclass
		[ApiExplorerSettings(IgnoreApi = true)]
		public abstract void UpdateEntity(ref TEntity entity, TViewModel viewModel);

		//Virtual method to be overrided by clients, if required extra configuration
		[ApiExplorerSettings(IgnoreApi = true)]
		public virtual void DeleteEntity(int id)
		{
		}
	}
}
