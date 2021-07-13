using Microsoft.EntityFrameworkCore.Migrations;

namespace InternshipProgressTracker.Migrations
{
    public partial class Students : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_InternshipStreams_InternshipStreamId",
                table: "Students");

            migrationBuilder.AlterColumn<int>(
                name: "InternshipStreamId",
                table: "Students",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CurrentGrade",
                table: "Students",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_InternshipStreams_InternshipStreamId",
                table: "Students",
                column: "InternshipStreamId",
                principalTable: "InternshipStreams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_InternshipStreams_InternshipStreamId",
                table: "Students");

            migrationBuilder.AlterColumn<int>(
                name: "InternshipStreamId",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CurrentGrade",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_InternshipStreams_InternshipStreamId",
                table: "Students",
                column: "InternshipStreamId",
                principalTable: "InternshipStreams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
