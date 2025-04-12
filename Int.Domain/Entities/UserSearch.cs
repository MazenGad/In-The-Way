using System;
using System.Collections.Generic;

namespace Int.Domain.Entities;

public partial class UserSearch
{
    public int USsn { get; set; }

    public int? SId { get; set; }

    public virtual SearchHistory? SIdNavigation { get; set; }

    public virtual User USsnNavigation { get; set; } = null!;
}
