using Microsoft.AspNetCore.Identity;

namespace Int.Domain.Entities;

public partial class User : IdentityUser
{

    public string firstName { get; set; }
    public string lastName { get; set; }
    public string? SSN { get; set; }
	public string? imageUrl { get; set; }

	public virtual ICollection<Car> Cars { get; set; }
}
