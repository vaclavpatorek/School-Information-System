using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolIS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "User",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "User",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Login",
                table: "User",
                column: "Login",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_Login",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Login",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "User");
        }
    }
}
