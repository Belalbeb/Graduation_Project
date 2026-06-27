using Graduation_Project.Models;

namespace Graduation_Project.Seeds
{
    public class SkillSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            if (context.Skills.Any())
                return;

            var skillNames = new[]
            {
                // Backend
                "C#", "ASP.NET Core", "Entity Framework Core", "SQL Server",
                "PostgreSQL", "Redis", "Node.js", "Python", "Java", "Go",
                // Frontend
                "JavaScript", "TypeScript", "React.js", "Angular", "Vue.js",
                "HTML5", "CSS3", "Tailwind CSS",
                // Mobile
                "Flutter", "React Native", "Swift", "Kotlin",
                // DevOps & Cloud
                "Docker", "Kubernetes", "CI/CD", "AWS", "Azure", "GCP",
                "Terraform", "Linux",
                // Data & AI
                "Machine Learning", "TensorFlow", "PyTorch", "Data Analysis",
                "Power BI", "Pandas",
                // General
                "REST API", "GraphQL", "Git & GitHub", "System Design",
                "Problem Solving", "OOP", "Design Patterns", "LINQ",
                "Agile / Scrum", "Microservices", "Clean Architecture"
            };

            var skills = skillNames
                .Distinct()
                .Select(name => new Skill { SkillName = name })
                .ToList();

            context.Skills.AddRange(skills);
            await context.SaveChangesAsync();
        }
    }
}
