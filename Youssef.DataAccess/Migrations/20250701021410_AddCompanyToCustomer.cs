using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Youssef.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "companyID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_companyID",
                table: "AspNetUsers",
                column: "companyID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Companies_companyID",
                table: "AspNetUsers",
                column: "companyID",
                principalTable: "Companies",
                principalColumn: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Companies_companyID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_companyID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "companyID",
                table: "AspNetUsers");
        }
    }
}
