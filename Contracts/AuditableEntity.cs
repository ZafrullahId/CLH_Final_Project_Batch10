using System;
namespace Dansnom.Contracts
{
    public class AuditableEntity : BaseEntity, IAuditableEntity, ISoftDelete
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get ; set; } = DateTime.Now;
        public int LastModifiedBy { get ; set ; }
        public DateTime LastModifiedOn { get ; set ; } = DateTime.Now;
        public DateTime? DeletedOn { get ; set; } = DateTime.Now;
        public int? DeletedBy { get ; set ; }
        public bool IsDeleted { get ; set ; } = false;
        DateTime? IAuditableEntity.CreatedOn { get; set; } = DateTime.Now;
        DateTime? IAuditableEntity.LastModifiedOn { get; set; } = DateTime.Now;
    }
}
