using Microsoft.EntityFrameworkCore.Migrations;

namespace EduHome.Migrations
{
    public partial class AddIsDeactiveToCourses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDeactive",
                table: "About");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeactive",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeactive",
                table: "Courses");

            migrationBuilder.AddColumn<bool>(
                name: "isDeactive",
                table: "About",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
