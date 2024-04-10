using System;
using System.Collections.Generic;

namespace INTEXII.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public int Year { get; set; }

    public int NumParts { get; set; }

    public int Price { get; set; }

    public string? ImgLink { get; set; }

    public string? PrimaryColor { get; set; }

    public string? SecondaryColor { get; set; }

    public string? Description { get; set; }

    public string? Category { get; set; }

    public string? PublicCategory { get; set; }

    public int? Recommendation1 { get; set; }

    public double? Similarity1 { get; set; }

    public int? Recommendation2 { get; set; }

    public double? Similarity2 { get; set; }

    public int? Recommendation3 { get; set; }

    public double? Similarity3 { get; set; }

    public int? Recommendation4 { get; set; }

    public double? Similarity4 { get; set; }

    public int? Recommendation5 { get; set; }

    public double? Similarity5 { get; set; }

    public double? PopularityRank { get; set; }
}
