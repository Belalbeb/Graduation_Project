using Graduation_Project.Models;
using System;

namespace Graduation_Project.Seeds
{
    public class SkillSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            if (context.Skills.Any())
                return;

            var skills = new List<Skill>
            {
                new Skill { SkillName = "C#" },
                new Skill { SkillName = "ASP.NET Core" },
                new Skill { SkillName = "Entity Framework Core" },
                new Skill { SkillName = "SQL Server" },
                new Skill { SkillName = "JavaScript" },
                new Skill { SkillName = "TypeScript" },
                new Skill { SkillName = "React.js" },
                new Skill { SkillName = "Angular" },
                new Skill { SkillName = "Node.js" },
                new Skill { SkillName = "Docker" },
                new Skill { SkillName = "Kubernetes" },
                new Skill { SkillName = "REST API" },
                new Skill { SkillName = "Git & GitHub" },
                new Skill { SkillName = "System Design" },
                new Skill { SkillName = "Problem Solving" },
                new Skill { SkillName = "OOP" },
                new Skill { SkillName = "Design Patterns" },
                new Skill { SkillName = "LINQ" }
            };

            context.Skills.AddRange(skills);
            await context.SaveChangesAsync();
        }
    }
}