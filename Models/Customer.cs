using System;
using System.Collections.Generic;

namespace INTEXII.Models;

public partial class Customer
{
    public int? CustomerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string BirthDate { get; set; } = null!;

    public string? CountryOfResidence { get; set; }

    public string? Gender { get; set; }

    public int? Age { get; set; }

    public string? Username { get; set; }
}
