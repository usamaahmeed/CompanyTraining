using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyTraining.Migrations
{
    /// <inheritdoc />
    public partial class ModifyRelationBetweenCertificateAndUserCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserCourse_CertificateId",
                table: "UserCourse");

            migrationBuilder.RenameColumn(
                name: "UserCourseId",
                table: "Certificate",
                newName: "CourseId");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Certificate",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserCourse_CertificateId",
                table: "UserCourse",
                column: "CertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_ApplicationUserId",
                table: "Certificate",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_CourseId",
                table: "Certificate",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificate_AspNetUsers_ApplicationUserId",
                table: "Certificate",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificate_Course_CourseId",
                table: "Certificate",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_AspNetUsers_ApplicationUserId",
                table: "Certificate");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_Course_CourseId",
                table: "Certificate");

            migrationBuilder.DropIndex(
                name: "IX_UserCourse_CertificateId",
                table: "UserCourse");

            migrationBuilder.DropIndex(
                name: "IX_Certificate_ApplicationUserId",
                table: "Certificate");

            migrationBuilder.DropIndex(
                name: "IX_Certificate_CourseId",
                table: "Certificate");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Certificate");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Certificate",
                newName: "UserCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCourse_CertificateId",
                table: "UserCourse",
                column: "CertificateId",
                unique: true,
                filter: "[CertificateId] IS NOT NULL");
        }
    }
}
