namespace Graduation_Project.Dtos
{
    public class CompanyDto
    {
        public string Email { get; set; }
        public string Password { get; set; }

        // Company profile fields
        public string Name { get; set; }
        public string Industry { get; set; }
        public string WebsiteURL { get; set; }
        public string HeadquarterAddress { get; set; }
        public string Location { get; set; }
        public string Logo { get; set; }
    }
}
