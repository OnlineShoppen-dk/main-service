﻿namespace main_service.Models.ApiModels.ProductApiModels;

public class PostProductRequest
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal? Price { get; set; }
    public int? Stock { get; set; }
    public int? Sold { get; set; }
    public bool Disabled { get; set; }
}