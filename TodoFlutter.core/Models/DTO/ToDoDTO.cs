using System;


namespace TodoFlutter.core.Models.DTO
{
    public class ToDoDTO
    {
        public int Id { get; set; }
        public string Task { get; set; }
        public bool Completed { get; set; } = false;
        public DateTime Date { get; set; }
        public string UserId { get; set; }
    }
}
