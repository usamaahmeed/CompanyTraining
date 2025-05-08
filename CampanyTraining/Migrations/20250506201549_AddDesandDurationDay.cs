using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyTraining.Migrations
{
    /// <inheritdoc />
    public partial class AddDesandDurationDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Package",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DurationDay",
                table: "Package",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "DurationDay",
                table: "Package");
        }
    }
}
