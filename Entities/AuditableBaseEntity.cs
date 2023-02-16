using System;
using System.ComponentModel.DataAnnotations;

namespace MovieAPi.Entities
{
    public abstract class AuditableBaseEntity
    {
        [Key]
        public virtual int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}