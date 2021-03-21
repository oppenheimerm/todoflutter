using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoFlutter.core.Models
{
    public class Todo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Task { get; set; }

        public bool Completed { get; set; } = false;

        public DateTime Date { get; set; }

        public AppUser User { get; set; }

        public string UserId { get; set; }
    }
}
