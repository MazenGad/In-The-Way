using System;
using System.Collections.Generic;

namespace Int.Domain.Entities;

public partial class SearchHistory
{
    public int SId { get; set; }

    public DateTime DateTime { get; set; }

    public string Notification { get; set; } = null!;

    public virtual ICollection<SearchCar> SearchCars { get; set; } = new List<SearchCar>();

    public virtual ICollection<UserSearch> UserSearches { get; set; } = new List<UserSearch>();
}
