using Microsoft.EntityFrameworkCore.Migrations;

namespace InternshipProgressTracker.Database.Migrations
{
    public partial class PhotoId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoName",
                table: "AspNetUsers",
                newName: "PhotoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoId",
                table: "AspNetUsers",
                newName: "PhotoName");
        }
    }
}
