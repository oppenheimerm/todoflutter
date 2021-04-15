using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoFlutter.core.Models
{
    public class Todo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(240)]
        public string Task { get; set; }

        public bool Completed { get; set; } = false;

        public DateTime Date { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual  AppUser User { get; set; }
    }
}
