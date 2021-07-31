using Microsoft.EntityFrameworkCore.Migrations;

namespace InternshipProgressTracker.Database.Migrations
{
    public partial class PhotoType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoType",
                table: "AspNetUsers");
        }
    }
}
