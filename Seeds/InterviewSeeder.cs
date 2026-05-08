using Bogus;
using Graduation_Project.Models;
using System;

namespace Graduation_Project.Seeds
{
    public class InterviewSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            if (context.Interviews.Any())
                return;

            var applicants = context.Applicants.ToList();
            var jobs = context.JobPostings.ToList();

            if (!applicants.Any() || !jobs.Any())
                return;

            var statuses = new[] { InterviewStatus.Upcoming, InterviewStatus.Completed, InterviewStatus.Cancelled };

            var faker = new Faker<Interview>()
                .RuleFor(i => i.ApplicantId,
                    f => f.PickRandom(applicants).ApplicantID)

                .RuleFor(i => i.JobPostingId,
                    f => f.PickRandom(jobs).JobID)

                .RuleFor(i => i.ScheduledAt,
                    f => f.Date.Future(1))

                .RuleFor(i => i.Status,
                    f => f.PickRandom(statuses))

                .RuleFor(i => i.InterviewerName,
                    f => f.Name.FullName())

                .RuleFor(i => i.MeetingLink,
                    f => "https://meet.google.com/" + f.Random.AlphaNumeric(10))

                .RuleFor(i => i.Notes,
                    f => f.Random.Bool()
                        ? f.Lorem.Sentence()
                        : null);

            var interviews = faker.Generate(25);

            context.Interviews.AddRange(interviews);
            await context.SaveChangesAsync();
        }
    }
}