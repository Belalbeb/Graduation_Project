using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Migrations
{
    /// <inheritdoc />
    public partial class jobskill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobSkill_JobPostings_JobPostingId",
                table: "JobSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobSkill",
                table: "JobSkill");

            migrationBuilder.RenameTable(
                name: "JobSkill",
                newName: "JobSkills");

            migrationBuilder.RenameIndex(
                name: "IX_JobSkill_JobPostingId",
                table: "JobSkills",
                newName: "IX_JobSkills_JobPostingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobSkills",
                table: "JobSkills",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobSkills_JobPostings_JobPostingId",
                table: "JobSkills",
                column: "JobPostingId",
                principalTable: "JobPostings",
                principalColumn: "JobID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobSkills_JobPostings_JobPostingId",
                table: "JobSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobSkills",
                table: "JobSkills");

            migrationBuilder.RenameTable(
                name: "JobSkills",
                newName: "JobSkill");

            migrationBuilder.RenameIndex(
                name: "IX_JobSkills_JobPostingId",
                table: "JobSkill",
                newName: "IX_JobSkill_JobPostingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobSkill",
                table: "JobSkill",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobSkill_JobPostings_JobPostingId",
                table: "JobSkill",
                column: "JobPostingId",
                principalTable: "JobPostings",
                principalColumn: "JobID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
