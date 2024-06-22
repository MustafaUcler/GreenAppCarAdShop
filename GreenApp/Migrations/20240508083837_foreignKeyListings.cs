using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenApp.Migrations
{
    /// <inheritdoc />
    public partial class foreignKeyListings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Listings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Listings_UserId",
                table: "Listings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Users_UserId",
                table: "Listings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Users_UserId",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_UserId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Listings");
        }
    }
}
