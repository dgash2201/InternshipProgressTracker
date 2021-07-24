using Microsoft.EntityFrameworkCore.Migrations;

namespace InternshipProgressTracker.Migrations
{
    public partial class StreamStudentsMentors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mentors_InternshipStreams_InternshipStreamId",
                table: "Mentors");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_InternshipStreams_InternshipStreamId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_InternshipStreamId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Mentors_InternshipStreamId",
                table: "Mentors");

            migrationBuilder.DropColumn(
                name: "InternshipStreamId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "InternshipStreamId",
                table: "Mentors");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Mentors");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Mentors");

            migrationBuilder.CreateTable(
                name: "InternshipStreamMentor",
                columns: table => new
                {
                    InternshipStreamsId = table.Column<int>(type: "int", nullable: false),
                    MentorsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipStreamMentor", x => new { x.InternshipStreamsId, x.MentorsId });
                    table.ForeignKey(
                        name: "FK_InternshipStreamMentor_InternshipStreams_InternshipStreamsId",
                        column: x => x.InternshipStreamsId,
                        principalTable: "InternshipStreams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InternshipStreamMentor_Mentors_MentorsId",
                        column: x => x.MentorsId,
                        principalTable: "Mentors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InternshipStreamStudent",
                columns: table => new
                {
                    InternshipStreamsId = table.Column<int>(type: "int", nullable: false),
                    StudentsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipStreamStudent", x => new { x.InternshipStreamsId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_InternshipStreamStudent_InternshipStreams_InternshipStreamsId",
                        column: x => x.InternshipStreamsId,
                        principalTable: "InternshipStreams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InternshipStreamStudent_Students_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InternshipStreamMentor_MentorsId",
                table: "InternshipStreamMentor",
                column: "MentorsId");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipStreamStudent_StudentsId",
                table: "InternshipStreamStudent",
                column: "StudentsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InternshipStreamMentor");

            migrationBuilder.DropTable(
                name: "InternshipStreamStudent");

            migrationBuilder.AddColumn<int>(
                name: "InternshipStreamId",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InternshipStreamId",
                table: "Mentors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Mentors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Mentors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Students_InternshipStreamId",
                table: "Students",
                column: "InternshipStreamId");

            migrationBuilder.CreateIndex(
                name: "IX_Mentors_InternshipStreamId",
                table: "Mentors",
                column: "InternshipStreamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mentors_InternshipStreams_InternshipStreamId",
                table: "Mentors",
                column: "InternshipStreamId",
                principalTable: "InternshipStreams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_InternshipStreams_InternshipStreamId",
                table: "Students",
                column: "InternshipStreamId",
                principalTable: "InternshipStreams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
