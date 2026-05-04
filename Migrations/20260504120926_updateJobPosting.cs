using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Migrations
{
    /// <inheritdoc />
    public partial class updateJobPosting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobType",
                table: "JobPostings");

            migrationBuilder.RenameColumn(
                name: "SalaryRange",
                table: "JobPostings",
                newName: "WorkApproaches");

            migrationBuilder.RenameColumn(
                name: "Requirements",
                table: "JobPostings",
                newName: "Responsibility");

            migrationBuilder.AddColumn<string>(
                name: "JobCategory",
                table: "JobPostings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "JobTypes",
                table: "JobPostings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "JobPostings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "MaxSalary",
                table: "JobPostings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinSalary",
                table: "JobPostings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "JobSkill",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobPostingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSkill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobSkill_JobPostings_JobPostingId",
                        column: x => x.JobPostingId,
                        principalTable: "JobPostings",
                        principalColumn: "JobID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobSkill_JobPostingId",
                table: "JobSkill",
                column: "JobPostingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobSkill");

            migrationBuilder.DropColumn(
                name: "JobCategory",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "JobTypes",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "MaxSalary",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "MinSalary",
                table: "JobPostings");

            migrationBuilder.RenameColumn(
                name: "WorkApproaches",
                table: "JobPostings",
                newName: "SalaryRange");

            migrationBuilder.RenameColumn(
                name: "Responsibility",
                table: "JobPostings",
                newName: "Requirements");

            migrationBuilder.AddColumn<int>(
                name: "JobType",
                table: "JobPostings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
