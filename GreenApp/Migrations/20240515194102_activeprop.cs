using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenApp.Migrations
{
    /// <inheritdoc />
    public partial class activeprop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Active",
                table: "Listings",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Listings");
        }
    }
}
