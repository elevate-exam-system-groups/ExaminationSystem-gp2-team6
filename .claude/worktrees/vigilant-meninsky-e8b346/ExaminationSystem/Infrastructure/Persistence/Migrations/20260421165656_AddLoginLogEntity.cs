using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExaminationSystem.Infrastructure.Persistence.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddLoginLogEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "StudentDiplomaEnrollments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StudentDiplomaEnrollments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<double>(
                name: "Score",
                table: "QuizAttempts",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "Passed",
                table: "QuizAttempts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedAt",
                table: "QuizAttempts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "QuizAttempts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AttemptAnswer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttemptId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    SelectedOptionId = table.Column<int>(type: "int", nullable: true),
                    AnsweredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuizAttemptId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttemptAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttemptAnswer_AnswerOptions_SelectedOptionId",
                        column: x => x.SelectedOptionId,
                        principalTable: "AnswerOptions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AttemptAnswer_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttemptAnswer_QuizAttempts_QuizAttemptId",
                        column: x => x.QuizAttemptId,
                        principalTable: "QuizAttempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttemptResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttemptId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    StudentAnswerOptionId = table.Column<int>(type: "int", nullable: true),
                    CorrectAnswerOptionId = table.Column<int>(type: "int", nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    QuizAttemptId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttemptResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttemptResult_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttemptResult_QuizAttempts_QuizAttemptId",
                        column: x => x.QuizAttemptId,
                        principalTable: "QuizAttempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoginLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoggedInAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoginLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttemptAnswer_QuestionId",
                table: "AttemptAnswer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AttemptAnswer_QuizAttemptId",
                table: "AttemptAnswer",
                column: "QuizAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_AttemptAnswer_SelectedOptionId",
                table: "AttemptAnswer",
                column: "SelectedOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_AttemptResult_QuestionId",
                table: "AttemptResult",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AttemptResult_QuizAttemptId",
                table: "AttemptResult",
                column: "QuizAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_LoginLogs_UserId",
                table: "LoginLogs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttemptAnswer");

            migrationBuilder.DropTable(
                name: "AttemptResult");

            migrationBuilder.DropTable(
                name: "LoginLogs");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StudentDiplomaEnrollments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "StudentDiplomaEnrollments");

            migrationBuilder.DropColumn(
                name: "Passed",
                table: "QuizAttempts");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "QuizAttempts");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "QuizAttempts");

            migrationBuilder.AlterColumn<int>(
                name: "Score",
                table: "QuizAttempts",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
