namespace ShopTARge24.Models.Kindergarten
{
    public class KindergartenDetailsViewModel
    {

        public Guid Id { get; set; }
        public string GroupName { get; set; }
        public int ChildrenCount { get; set; }
        public string KindergartenName { get; set; }
        public string TeacherName { get; set; }

        public List<KindergartenImageViewModel> Images { get; set; }
           = new List<KindergartenImageViewModel>();

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
