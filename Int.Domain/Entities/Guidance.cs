using System;
using System.Collections.Generic;

namespace Int.Domain.Entities;

public partial class Guidance
{
    public int GId { get; set; }

    public string GContent { get; set; } = null!;

    public int? ASsn { get; set; }

    public virtual Admin? ASsnNavigation { get; set; }

    public virtual ICollection<User> USsns { get; set; } = new List<User>();
}
