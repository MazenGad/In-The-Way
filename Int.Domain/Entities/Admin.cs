using System;
using System.Collections.Generic;

namespace Int.Domain.Entities;

public partial class Admin
{
    public int ASsn { get; set; }

    public string AName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<AdminMnageCar> AdminMnageCars { get; set; } = new List<AdminMnageCar>();

    public virtual ICollection<AdminPhone> AdminPhones { get; set; } = new List<AdminPhone>();

    public virtual ICollection<Guidance> Guidances { get; set; } = new List<Guidance>();

    public virtual ICollection<UserMessageSendReceive> UserMessageSendReceives { get; set; } = new List<UserMessageSendReceive>();

    public virtual ICollection<User> USsns { get; set; } = new List<User>();
}
