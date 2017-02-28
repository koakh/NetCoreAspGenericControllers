using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace NetCoreAspGenericControllers.Abstract
{
    //T:[Entity]ViewModel
    interface IGenericCrudController<TEntity, TViewModel>
		where TEntity : class, IEntityBase, new()
		where TViewModel : class, IViewModel, new()
	{
		IActionResult Get();

        IActionResult Get(int id);

        IActionResult Create([FromBody] TViewModel user);

        IActionResult Put(int id, [FromBody] TViewModel user);

        IActionResult Delete(int id);

		//Virtual
		IEnumerable<TEntity> GetEntities(int currentPage, int currentPageSize);

		TEntity GetEntity(int id);

		TEntity CreateEntity(TViewModel viewModel);

		void UpdateEntity(ref TEntity entity, TViewModel viewModel);

		void DeleteEntity(int id);
	}
}
