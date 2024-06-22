using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Username", "PasswordHash", "EmailAddress", "Role" },
                values: new object[] { "Admin", BCrypt.Net.BCrypt.EnhancedHashPassword("Admin123"), "Admin@admin.se", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "Admin");
        }
    }
}

//This is a migration to automatically create an admin account upon update-database so
//when someone is to run this project for the first time, this account exists.