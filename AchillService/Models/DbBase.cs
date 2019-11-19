using System;
using System.ComponentModel.DataAnnotations;

namespace AchillService.Models
{
    public abstract class DbBase
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
