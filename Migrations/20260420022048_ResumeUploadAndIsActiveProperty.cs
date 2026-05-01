using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Migrations
{
    /// <inheritdoc />
    public partial class ResumeUploadAndIsActiveProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Resumes_ApplicantID",
                table: "Resumes");

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_ApplicantID_IsActive",
                table: "Resumes",
                columns: new[] { "ApplicantID", "IsActive" },
                unique: true,
                filter: "[IsActive] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Resumes_ApplicantID_IsActive",
                table: "Resumes");

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_ApplicantID",
                table: "Resumes",
                column: "ApplicantID");
        }
    }
}
