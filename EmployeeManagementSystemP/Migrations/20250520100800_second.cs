using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementSystemP.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeesLanguages_Languages_LanguageId1",
                table: "EmployeesLanguages");

            migrationBuilder.DropIndex(
                name: "IX_EmployeesLanguages_LanguageId1",
                table: "EmployeesLanguages");

            migrationBuilder.DropColumn(
                name: "LanguageId1",
                table: "EmployeesLanguages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LanguageId1",
                table: "EmployeesLanguages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeesLanguages_LanguageId1",
                table: "EmployeesLanguages",
                column: "LanguageId1");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeesLanguages_Languages_LanguageId1",
                table: "EmployeesLanguages",
                column: "LanguageId1",
                principalTable: "Languages",
                principalColumn: "LanguageId");
        }
    }
}
