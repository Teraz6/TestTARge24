namespace ShopTARge24.Models.Kindergarten
{
    public class KindergartenImageViewModel
    {
        public Guid ImageId { get; set; }
        public string? ImageTitle { get; set; }
        public string? Filepath { get; set; }
        public byte[]? ImageData { get; set; }
        public string? Image { get; set; }

        public Guid? KindergartenId { get; set; }
    }
}