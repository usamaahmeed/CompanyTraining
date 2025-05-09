using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyTraining.Migrations
{
    /// <inheritdoc />
    public partial class unifyUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_AspNetUsers_ApplicationCompanyId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscribe_AspNetUsers_ApplicationCompanyId",
                table: "Subscribe");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCourse_ApplicationUser_ApplicationUserId",
                table: "UserCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_UserQuizAttempt_ApplicationUser_ApplicationUserId",
                table: "UserQuizAttempt");

            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCourse",
                table: "UserCourse");

            migrationBuilder.RenameColumn(
                name: "ApplicationCompanyId",
                table: "Subscribe",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscribe_ApplicationCompanyId",
                table: "Subscribe",
                newName: "IX_Subscribe_ApplicationUserId");

            migrationBuilder.RenameColumn(
                name: "ApplicationCompanyId",
                table: "Category",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Category_ApplicationCompanyId",
                table: "Category",
                newName: "IX_Category_ApplicationUserId");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "UserQuizAttempt",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "UserCourse",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserCourse",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "MainImg",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CoverImg",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Roles",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCourse",
                table: "UserCourse",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserCourse_ApplicationUserId",
                table: "UserCourse",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Category_AspNetUsers_ApplicationUserId",
                table: "Category",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribe_AspNetUsers_ApplicationUserId",
                table: "Subscribe",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourse_AspNetUsers_ApplicationUserId",
                table: "UserCourse",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuizAttempt_AspNetUsers_ApplicationUserId",
                table: "UserQuizAttempt",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_CompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Category_AspNetUsers_ApplicationUserId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscribe_AspNetUsers_ApplicationUserId",
                table: "Subscribe");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCourse_AspNetUsers_ApplicationUserId",
                table: "UserCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_UserQuizAttempt_AspNetUsers_ApplicationUserId",
                table: "UserQuizAttempt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCourse",
                table: "UserCourse");

            migrationBuilder.DropIndex(
                name: "IX_UserCourse_ApplicationUserId",
                table: "UserCourse");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserCourse");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Roles",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Subscribe",
                newName: "ApplicationCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscribe_ApplicationUserId",
                table: "Subscribe",
                newName: "IX_Subscribe_ApplicationCompanyId");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Category",
                newName: "ApplicationCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Category_ApplicationUserId",
                table: "Category",
                newName: "IX_Category_ApplicationCompanyId");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "UserQuizAttempt",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "UserCourse",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "MainImg",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CoverImg",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCourse",
                table: "UserCourse",
                columns: new[] { "ApplicationUserId", "CourseId" });

            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUser_AspNetUsers_ApplicationCompanyId",
                        column: x => x.ApplicationCompanyId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_ApplicationCompanyId",
                table: "ApplicationUser",
                column: "ApplicationCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_Email",
                table: "ApplicationUser",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Category_AspNetUsers_ApplicationCompanyId",
                table: "Category",
                column: "ApplicationCompanyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribe_AspNetUsers_ApplicationCompanyId",
                table: "Subscribe",
                column: "ApplicationCompanyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourse_ApplicationUser_ApplicationUserId",
                table: "UserCourse",
                column: "ApplicationUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuizAttempt_ApplicationUser_ApplicationUserId",
                table: "UserQuizAttempt",
                column: "ApplicationUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");
        }
    }
}
