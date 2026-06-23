namespace Graduation_Project.Dtos
{
    public class JobCardDto
    {
       
        public string CompanyName { get; set; }
        public string CompanyLogoUrl { get; set; }
        public string Location { get; set; }
        public string? Category { get; set; }

        public bool IsApplied { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

 
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }

        public DateTime PostedDate { get; set; }


        public List<string> JobTypes { get; set; } = new();
        public List<string> WorkApproaches { get; set; } = new();

        public Guid JobID { get; set; }
    }
}
