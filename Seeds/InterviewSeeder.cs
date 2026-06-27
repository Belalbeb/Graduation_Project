using Bogus;
using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Seeds
{
    public class InterviewSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            if (context.Interviews.Any())
                return;

            var applications = await context.Applications
                .Where(a => a.ApplicationStatus == ApplicationStatus.Accepted
                         || a.ApplicationStatus == ApplicationStatus.Reviewed
                         || a.ApplicationStatus == ApplicationStatus.Pending)
                .ToListAsync();

            if (!applications.Any())
                return;

            var faker = new Faker();
            var types = Enum.GetValues<InterviewType>();
            var interviews = new List<Interview>();

            foreach (var application in applications)
            {
                if (!faker.Random.Bool(0.45f))
                    continue;

                // Interview scheduled after application date
                var interviewDate = DateOnly.FromDateTime(
                    faker.Date.Between(application.AppliedDate.AddDays(3), DateTime.UtcNow.AddDays(30)));

                var startHour = faker.Random.Int(9, 16);
                var startMinute = faker.PickRandom(new[] { 0, 30 }); // on the hour or half-hour
                var startTime = new TimeOnly(startHour, startMinute);
                var duration = faker.PickRandom(new[] { 30, 45, 60, 90 });

                var interviewType = faker.PickRandom(types);

                interviews.Add(new Interview
                {
                    ApplicantId = application.ApplicantID,
                    JobPostingId = application.JobPostingID,
                    InterviewDate = interviewDate,
                    StartTime = startTime,
                    EndTime = startTime.AddMinutes(duration),
                    Status = application.ApplicationStatus == ApplicationStatus.Accepted
                        ? InterviewStatus.Upcoming
                        : faker.PickRandom<InterviewStatus>(),
                    interviewType = interviewType,
                    InterviewerName = faker.Name.FullName(),
                    InterviewerPosition = faker.Name.JobTitle(),
                    MeetingLink =
                         "https://meet.google.com/" + faker.Random.AlphaNumeric(10),


                    Notes = faker.Random.Bool(0.5f) ? faker.Lorem.Sentence() : null
                });
            }

            context.Interviews.AddRange(interviews);
            await context.SaveChangesAsync();
        }
    }
}
