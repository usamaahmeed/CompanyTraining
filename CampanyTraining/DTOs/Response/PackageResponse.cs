﻿namespace CompanyTraining.DTOs.Response
{
    public class PackageResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;

        public int DurationDay { get; set; }
    }
}
