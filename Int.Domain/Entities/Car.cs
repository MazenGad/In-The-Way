namespace Int.Domain.Entities;

public partial class Car
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public string Location { get; set; } = null!;

    public string? PlateNumber { get; set; }

    public int ColorId { get; set; }
    public Color Color { get; set; }
	public int BrandId { get; set; }

	public Brand Brand { get; set; }
	public int ModelId { get; set; }

	public Model Model { get; set; }
    public DateTime CreateAt { get; set; } =DateTime.UtcNow;


    public bool IsDeleted { get; set; } = false;

	public virtual ICollection<CarPhoto> CarPhotos { get; set; } = new List<CarPhoto>();
    public string UserId { get; set; }
    public virtual User? User { get; set; }
}
