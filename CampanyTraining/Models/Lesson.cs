﻿namespace CompanyTraining.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string VideoUrl { get; set; } = null!;
        public int ModuleId { get; set; }
        public Module Module { get; set; } = null!;

    }
}
