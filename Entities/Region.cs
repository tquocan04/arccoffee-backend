﻿namespace Entities
{
    public class Region
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public ICollection<City>? Cities { get; set; }
    }
}
