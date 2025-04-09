using System;
using System.Collections.Generic;

namespace Int.Domain.Entities;

public partial class Message
{
    public int MId { get; set; }

    public DateTime DateTime { get; set; }

    public string Content { get; set; } = null!;

    public virtual ICollection<UserMessageSendReceive> UserMessageSendReceives { get; set; } = new List<UserMessageSendReceive>();
}
