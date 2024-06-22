using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenApp.Migrations
{
    /// <inheritdoc />
    public partial class guidcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GUID",
                table: "Listings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GUID",
                table: "Listings");
        }
    }
}
