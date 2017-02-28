using System.ComponentModel.DataAnnotations;

namespace NetCoreAspGenericControllers.Abstract
{
    public interface IViewModel : IValidatableObject
    {
        int Id { get; set; }
    }
}
