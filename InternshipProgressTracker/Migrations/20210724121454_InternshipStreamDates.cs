using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternshipProgressTracker.Migrations
{
    public partial class InternshipStreamDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "InternshipStreams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FactEndDate",
                table: "InternshipStreams",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FactStartDate",
                table: "InternshipStreams",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PlanEndDate",
                table: "InternshipStreams",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PlanStartDate",
                table: "InternshipStreams",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FactEndDate",
                table: "InternshipStreams");

            migrationBuilder.DropColumn(
                name: "FactStartDate",
                table: "InternshipStreams");

            migrationBuilder.DropColumn(
                name: "PlanEndDate",
                table: "InternshipStreams");

            migrationBuilder.DropColumn(
                name: "PlanStartDate",
                table: "InternshipStreams");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "InternshipStreams",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
