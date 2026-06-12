using Bogus;
using Graduation_Project.Models;

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

            var statuses = Enum.GetValues<InterviewStatus>();
            var types = Enum.GetValues<InterviewType>();

            var faker = new Faker<Interview>()
                .RuleFor(i => i.ApplicantId,
                    f => f.PickRandom(applicants).ApplicantID)

                .RuleFor(i => i.JobPostingId,
                    f => f.PickRandom(jobs).JobID)

                // 📅 Date (next 30 days)
                .RuleFor(i => i.InterviewDate,
                    f => DateOnly.FromDateTime(f.Date.Soon(30)))

                // ⏰ Start time (9 AM - 5 PM)
                .RuleFor(i => i.StartTime,
                    f => TimeOnly.FromTimeSpan(TimeSpan.FromHours(f.Random.Int(9, 16))))

                // ⏰ End time (30–90 min later)
                .RuleFor(i => i.EndTime,
                    (f, i) =>
                    {
                        var duration = f.Random.Int(30, 90);
                        return i.StartTime.AddMinutes(duration);
                    })

                .RuleFor(i => i.Status,
                    f => f.PickRandom(statuses))

                .RuleFor(i => i.interviewType,
                    f => f.PickRandom(types))

                .RuleFor(i => i.InterviewerName,
                    f => f.Name.FullName())

                .RuleFor(i => i.InterviewerPosition,
                    f => f.Name.JobTitle())

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