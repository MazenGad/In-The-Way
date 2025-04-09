using Microsoft.AspNetCore.Identity;

namespace Int.Domain.Entities;

public partial class User : IdentityUser
{

    public string firstName { get; set; }
    public string lastName { get; set; }

    //public virtual ICollection<UserMessageSendReceive> UserMessageSendReceives { get; set; } = new List<UserMessageSendReceive>();

    //public virtual UserSearch? UserSearch { get; set; }

    //public virtual ICollection<Admin> ASsns { get; set; } = new List<Admin>();

    //public virtual ICollection<Car> CIds { get; set; } = new List<Car>();

    //public virtual ICollection<Guidance> GIds { get; set; } = new List<Guidance>();
}
