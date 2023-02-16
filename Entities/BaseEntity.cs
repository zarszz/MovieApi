using System;
using System.ComponentModel.DataAnnotations;

namespace MovieAPi.Entities
{
    public class BaseEntity
    {
        [Key]
        public virtual int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}