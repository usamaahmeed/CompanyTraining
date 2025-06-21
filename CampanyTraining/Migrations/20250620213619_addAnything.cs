using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyTraining.Migrations
{
    /// <inheritdoc />
    public partial class addAnything : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswer_UserQuizAttempt_UserQuizAttemptId",
                table: "UserAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_UserQuizAttempt_AspNetUsers_ApplicationUserId",
                table: "UserQuizAttempt");

            migrationBuilder.DropForeignKey(
                name: "FK_UserQuizAttempt_Quiz_QuizId",
                table: "UserQuizAttempt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserQuizAttempt",
                table: "UserQuizAttempt");

            migrationBuilder.RenameTable(
                name: "UserQuizAttempt",
                newName: "UserQuizAttempts");

            migrationBuilder.RenameIndex(
                name: "IX_UserQuizAttempt_QuizId",
                table: "UserQuizAttempts",
                newName: "IX_UserQuizAttempts_QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_UserQuizAttempt_ApplicationUserId",
                table: "UserQuizAttempts",
                newName: "IX_UserQuizAttempts_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserQuizAttempts",
                table: "UserQuizAttempts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswer_UserQuizAttempts_UserQuizAttemptId",
                table: "UserAnswer",
                column: "UserQuizAttemptId",
                principalTable: "UserQuizAttempts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuizAttempts_AspNetUsers_ApplicationUserId",
                table: "UserQuizAttempts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuizAttempts_Quiz_QuizId",
                table: "UserQuizAttempts",
                column: "QuizId",
                principalTable: "Quiz",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswer_UserQuizAttempts_UserQuizAttemptId",
                table: "UserAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_UserQuizAttempts_AspNetUsers_ApplicationUserId",
                table: "UserQuizAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserQuizAttempts_Quiz_QuizId",
                table: "UserQuizAttempts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserQuizAttempts",
                table: "UserQuizAttempts");

            migrationBuilder.RenameTable(
                name: "UserQuizAttempts",
                newName: "UserQuizAttempt");

            migrationBuilder.RenameIndex(
                name: "IX_UserQuizAttempts_QuizId",
                table: "UserQuizAttempt",
                newName: "IX_UserQuizAttempt_QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_UserQuizAttempts_ApplicationUserId",
                table: "UserQuizAttempt",
                newName: "IX_UserQuizAttempt_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserQuizAttempt",
                table: "UserQuizAttempt",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswer_UserQuizAttempt_UserQuizAttemptId",
                table: "UserAnswer",
                column: "UserQuizAttemptId",
                principalTable: "UserQuizAttempt",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuizAttempt_AspNetUsers_ApplicationUserId",
                table: "UserQuizAttempt",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuizAttempt_Quiz_QuizId",
                table: "UserQuizAttempt",
                column: "QuizId",
                principalTable: "Quiz",
                principalColumn: "Id");
        }
    }
}
