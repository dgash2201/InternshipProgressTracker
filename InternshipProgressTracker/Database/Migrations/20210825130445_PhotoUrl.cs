using Microsoft.EntityFrameworkCore.Migrations;

namespace InternshipProgressTracker.Database.Migrations
{
    public partial class PhotoUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoId",
                table: "AspNetUsers",
                newName: "PhotoUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoUrl",
                table: "AspNetUsers",
                newName: "PhotoId");
        }
    }
}
