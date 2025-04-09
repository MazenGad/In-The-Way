using System;
using System.Collections.Generic;

namespace Int.Domain.Entities;

public partial class SearchCar
{
    public int? SId { get; set; }

    public int CId { get; set; }

    public virtual Car CIdNavigation { get; set; } = null!;

    public virtual SearchHistory? SIdNavigation { get; set; }
}
