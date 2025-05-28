using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyTraining.Migrations
{
    /// <inheritdoc />
    public partial class addStartEndDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalQuestions",
                table: "UserQuizAttempt");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "UserQuizAttempt",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "UserQuizAttempt",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsGenerated",
                table: "Quiz",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "UserQuizAttempt");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "UserQuizAttempt");

            migrationBuilder.DropColumn(
                name: "IsGenerated",
                table: "Quiz");

            migrationBuilder.AddColumn<int>(
                name: "TotalQuestions",
                table: "UserQuizAttempt",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
