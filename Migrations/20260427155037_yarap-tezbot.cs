using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Migrations
{
    /// <inheritdoc />
    public partial class yaraptezbot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobType",
                table: "Experiences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Experiences",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AboutMe",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverPhotoUrl",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Facebook",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Github",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Linkedin",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Portfolio",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobType",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "AboutMe",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "CoverPhotoUrl",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "Facebook",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "Github",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "Linkedin",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "Portfolio",
                table: "Applicants");
        }
    }
}
