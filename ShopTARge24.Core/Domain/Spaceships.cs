namespace ShopTARge24.Core.Domain
{
    public class Spaceships
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Classification { get; set; }
        public DateTime? BuiltDate { get; set; }
        public int? Crew { get; set; }
        public int? EnginePower { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
