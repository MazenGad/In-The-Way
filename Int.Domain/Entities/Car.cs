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
    public DateTime CreateAt { get; set; } =DateTime.UtcNow;


   

    public virtual ICollection<CarPhoto> CarPhotos { get; set; } = new List<CarPhoto>();
    public string UserId { get; set; }
    public virtual User? User { get; set; }
}
