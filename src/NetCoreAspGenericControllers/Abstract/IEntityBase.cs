using System;

namespace NetCoreAspGenericControllers.Abstract
{
    public interface IEntityBase
    {
        int Id { get; set; }
        DateTime CreateDate { get; }
        DateTime EditDate { get; }
        DateTime DeleteDate { get; }
        bool IsDeleted { get; }
    }
}
