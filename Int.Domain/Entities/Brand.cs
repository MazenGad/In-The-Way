using System;
using System.Collections.Generic;

namespace Int.Domain.Entities;

public partial class Brand
{
    public int BCode { get; set; }

    public string BName { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();

    public virtual ICollection<Model> Models { get; set; } = new List<Model>();
}
