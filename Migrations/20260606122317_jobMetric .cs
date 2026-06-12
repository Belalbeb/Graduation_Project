using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Migrations
{
    /// <inheritdoc />
    public partial class jobMetric : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobMetrics_JobPostings_JobID",
                table: "JobMetrics");

            migrationBuilder.AddForeignKey(
                name: "FK_JobMetrics_JobPostings_JobID",
                table: "JobMetrics",
                column: "JobID",
                principalTable: "JobPostings",
                principalColumn: "JobID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobMetrics_JobPostings_JobID",
                table: "JobMetrics");

            migrationBuilder.AddForeignKey(
                name: "FK_JobMetrics_JobPostings_JobID",
                table: "JobMetrics",
                column: "JobID",
                principalTable: "JobPostings",
                principalColumn: "JobID");
        }
    }
}
