using Microsoft.EntityFrameworkCore.Migrations;

namespace InternshipProgressTracker.Migrations
{
    public partial class SoftDeletable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StudyPlans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StudyPlanEntries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StudentStudyPlanProgresses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Mentors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "InternshipStreams",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlans_IsDeleted",
                table: "StudyPlans",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanEntries_IsDeleted",
                table: "StudyPlanEntries",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_StudentStudyPlanProgresses_IsDeleted",
                table: "StudentStudyPlanProgresses",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Students_IsDeleted",
                table: "Students",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Mentors_IsDeleted",
                table: "Mentors",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipStreams_IsDeleted",
                table: "InternshipStreams",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IsDeleted",
                table: "AspNetUsers",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudyPlans_IsDeleted",
                table: "StudyPlans");

            migrationBuilder.DropIndex(
                name: "IX_StudyPlanEntries_IsDeleted",
                table: "StudyPlanEntries");

            migrationBuilder.DropIndex(
                name: "IX_StudentStudyPlanProgresses_IsDeleted",
                table: "StudentStudyPlanProgresses");

            migrationBuilder.DropIndex(
                name: "IX_Students_IsDeleted",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Mentors_IsDeleted",
                table: "Mentors");

            migrationBuilder.DropIndex(
                name: "IX_InternshipStreams_IsDeleted",
                table: "InternshipStreams");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "StudyPlans");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "StudyPlanEntries");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "StudentStudyPlanProgresses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Mentors");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "InternshipStreams");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");
        }
    }
}
