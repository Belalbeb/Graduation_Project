using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Migrations
{
    /// <inheritdoc />
    public partial class joppostingUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Resumes_ApplicantID",
                table: "Resumes");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Resumes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Interviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Resumes");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Interviews",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_ApplicantID",
                table: "Resumes",
                column: "ApplicantID");
        }
    }
}
