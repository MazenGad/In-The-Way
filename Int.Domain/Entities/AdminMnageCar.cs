using System;
using System.Collections.Generic;

namespace Int.Domain.Entities;

public partial class AdminMnageCar
{
    public int? ASsn { get; set; }

    public int CId { get; set; }

    public virtual Admin? ASsnNavigation { get; set; }

    public virtual Car CIdNavigation { get; set; } = null!;
}
