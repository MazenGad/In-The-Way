using System;
using System.Collections.Generic;

namespace Int.Domain.Entities;

public partial class Color
{
    public int CCode { get; set; }

    public string CName { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
