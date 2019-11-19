using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AchillService.Models
{
    public class Comment : DbBase
    {
        [Required]
        [ConcurrencyCheck]
        public string IssueId { get; set; }

        [ConcurrencyCheck]
        public string Username { get; set; }

        [ConcurrencyCheck]
        public DateTime CommentTime { get; set; } = DateTime.Now;
    }
}
