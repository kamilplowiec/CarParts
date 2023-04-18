namespace CarParts.Models.User
{
    public class UserFilterModel
    {
        public string FilterFirstName { get; set; }
        public string FilterLastName { get; set; }
        public string FilterUsername { get; set; }
        public string FilterEmail { get; set; }
        public int FilterAccountRole { get; set; }
    }
}