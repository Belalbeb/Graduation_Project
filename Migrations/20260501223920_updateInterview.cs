using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Migrations
{
    public partial class updateInterview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop old index if exists (safe)
            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT name 
    FROM sys.indexes 
    WHERE name = 'IX_Resumes_ApplicantID_IsActive'
)
BEGIN
    DROP INDEX IX_Resumes_ApplicantID_IsActive ON Resumes
END
");

            // Recreate correct filtered unique index
            migrationBuilder.CreateIndex(
                name: "IX_Resumes_ApplicantID_IsActive",
                table: "Resumes",
                columns: new[] { "ApplicantID", "IsActive" },
                unique: true,
                filter: "[IsActive] = 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop index safely
            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT name 
    FROM sys.indexes 
    WHERE name = 'IX_Resumes_ApplicantID_IsActive'
)
BEGIN
    DROP INDEX IX_Resumes_ApplicantID_IsActive ON Resumes
END
");

            // Optional: recreate non-unique index (or leave empty if you want)
            migrationBuilder.CreateIndex(
                name: "IX_Resumes_ApplicantID",
                table: "Resumes",
                column: "ApplicantID");
        }
    }
}