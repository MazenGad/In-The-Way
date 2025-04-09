using System;
using System.Collections.Generic;

namespace Int.Domain.Entities;

public partial class UserMessageSendReceive
{
    public int USsn { get; set; }

    public int MId { get; set; }

    public int ASsn { get; set; }

    public virtual Admin ASsnNavigation { get; set; } = null!;

    public virtual Message MIdNavigation { get; set; } = null!;

    public virtual User USsnNavigation { get; set; } = null!;
}
