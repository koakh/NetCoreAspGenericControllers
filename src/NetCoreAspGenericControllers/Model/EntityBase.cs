using NetCoreAspGenericControllers.Abstract;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAspGenericControllers.Model
{
    public class EntityBase : IEntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreateDate { get; } = DateTime.UtcNow;
        public DateTime EditDate { get; } = DateTime.UtcNow;
        public DateTime DeleteDate { get; } = DateTime.UtcNow;
        public bool IsDeleted { get; }

        public EntityBase()
        {
            CreateDate = DateTime.UtcNow;
            EditDate = DateTime.UtcNow;
            DeleteDate = DateTime.MinValue;
        }
    }
}
