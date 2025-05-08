using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyTraining.Migrations
{
    /// <inheritdoc />
    public partial class addCompositeInSubscribe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscribe",
                table: "Subscribe");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Subscribe");

            migrationBuilder.AlterColumn<string>(
                name: "SessionId",
                table: "Subscribe",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscribe",
                table: "Subscribe",
                columns: new[] { "SessionId", "ApplicationCompanyId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscribe",
                table: "Subscribe");

            migrationBuilder.AlterColumn<string>(
                name: "SessionId",
                table: "Subscribe",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Subscribe",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscribe",
                table: "Subscribe",
                column: "Id");
        }
    }
}
