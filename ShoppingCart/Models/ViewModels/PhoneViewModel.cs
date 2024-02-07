namespace PhoneStore.Models.ViewModels
{
    public class PhoneViewModel
    {
        public IEnumerable<Phone> Phones { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
        public String STerm { get; set; } = "";
        public int BrandId { get; set; } = 0;
        public string SortOrder { get; set; }
    }
}
