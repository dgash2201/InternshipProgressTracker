using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternshipProgressTracker.Migrations
{
    public partial class StudyPlanProgress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "StudentStudyPlanProgresses",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "GradingMentorId",
                table: "StudentStudyPlanProgresses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Grade",
                table: "StudentStudyPlanProgresses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FinishTime",
                table: "StudentStudyPlanProgresses",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "StudentId1",
                table: "StudentStudyPlanProgresses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudyPlanEntryId1",
                table: "StudentStudyPlanProgresses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentStudyPlanProgresses_StudentId1",
                table: "StudentStudyPlanProgresses",
                column: "StudentId1");

            migrationBuilder.CreateIndex(
                name: "IX_StudentStudyPlanProgresses_StudyPlanEntryId1",
                table: "StudentStudyPlanProgresses",
                column: "StudyPlanEntryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentStudyPlanProgresses_Students_StudentId1",
                table: "StudentStudyPlanProgresses",
                column: "StudentId1",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentStudyPlanProgresses_StudyPlanEntries_StudyPlanEntryId1",
                table: "StudentStudyPlanProgresses",
                column: "StudyPlanEntryId1",
                principalTable: "StudyPlanEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentStudyPlanProgresses_Students_StudentId1",
                table: "StudentStudyPlanProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentStudyPlanProgresses_StudyPlanEntries_StudyPlanEntryId1",
                table: "StudentStudyPlanProgresses");

            migrationBuilder.DropIndex(
                name: "IX_StudentStudyPlanProgresses_StudentId1",
                table: "StudentStudyPlanProgresses");

            migrationBuilder.DropIndex(
                name: "IX_StudentStudyPlanProgresses_StudyPlanEntryId1",
                table: "StudentStudyPlanProgresses");

            migrationBuilder.DropColumn(
                name: "StudentId1",
                table: "StudentStudyPlanProgresses");

            migrationBuilder.DropColumn(
                name: "StudyPlanEntryId1",
                table: "StudentStudyPlanProgresses");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "StudentStudyPlanProgresses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GradingMentorId",
                table: "StudentStudyPlanProgresses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Grade",
                table: "StudentStudyPlanProgresses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FinishTime",
                table: "StudentStudyPlanProgresses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
