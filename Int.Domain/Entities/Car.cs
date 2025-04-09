namespace Int.Domain.Entities;

public partial class Car
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public string Location { get; set; } = null!;

    public string? PlateNumber { get; set; }

    public string? Color { get; set; }

    public string? Brand { get; set; }
    public string? Model { get; set; }


    // public virtual AdminMnageCar? AdminMnageCar { get; set; }

    //public virtual Brand? BCodeNavigation { get; set; }

    //public virtual Color? CCodeNavigation { get; set; }

    public virtual ICollection<CarPhoto> CarPhotos { get; set; } = new List<CarPhoto>();

    //public virtual SearchCar? SearchCar { get; set; }

    //public virtual ICollection<User> USsns { get; set; } = new List<User>();
}
