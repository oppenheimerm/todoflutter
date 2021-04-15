using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoFlutter.core.Models.Request.ToDo
{
    /// <summary>
    /// This is just a "poco" class that is use in the creation of a single todo on the
    /// front-end. It prevents overposting by virtue of only taking the the properties
    /// we require from the user.
    /// </summary>
    public class ToDoRequest : BaseToDoRequest
    {
        public ToDoRequest(string userId) : base(userId)
        {

        }

        [Required]
        [StringLength(50)]
        public string Task { get; set; }
        public bool Completed { get; set; } = false;
    }
}
