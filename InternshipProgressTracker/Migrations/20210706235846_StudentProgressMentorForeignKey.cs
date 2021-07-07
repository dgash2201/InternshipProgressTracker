using Microsoft.EntityFrameworkCore.Migrations;

namespace InternshipProgressTracker.Migrations
{
    public partial class StudentProgressMentorForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentStudyPlanProgresses_Mentors_MentorId",
                table: "StudentStudyPlanProgresses");

            migrationBuilder.DropIndex(
                name: "IX_StudentStudyPlanProgresses_MentorId",
                table: "StudentStudyPlanProgresses");

            migrationBuilder.DropColumn(
                name: "MentorId",
                table: "StudentStudyPlanProgresses");

            migrationBuilder.CreateIndex(
                name: "IX_StudentStudyPlanProgresses_GradingMentorId",
                table: "StudentStudyPlanProgresses",
                column: "GradingMentorId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentStudyPlanProgresses_Mentors_GradingMentorId",
                table: "StudentStudyPlanProgresses",
                column: "GradingMentorId",
                principalTable: "Mentors",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentStudyPlanProgresses_Mentors_GradingMentorId",
                table: "StudentStudyPlanProgresses");

            migrationBuilder.DropIndex(
                name: "IX_StudentStudyPlanProgresses_GradingMentorId",
                table: "StudentStudyPlanProgresses");

            migrationBuilder.AddColumn<int>(
                name: "MentorId",
                table: "StudentStudyPlanProgresses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentStudyPlanProgresses_MentorId",
                table: "StudentStudyPlanProgresses",
                column: "MentorId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentStudyPlanProgresses_Mentors_MentorId",
                table: "StudentStudyPlanProgresses",
                column: "MentorId",
                principalTable: "Mentors",
                principalColumn: "Id");
        }
    }
}
