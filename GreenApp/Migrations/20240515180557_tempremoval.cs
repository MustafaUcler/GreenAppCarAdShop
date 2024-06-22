using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenApp.Migrations
{
    /// <inheritdoc />
    public partial class tempremoval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseHistory",
                columns: table => new
                {
                    PurchaseHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListingId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseHistory", x => x.PurchaseHistoryId);
                    table.ForeignKey(
                        name: "FK_PurchaseHistory_Listings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Listings",
                        principalColumn: "ListingId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseHistory_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseHistory_ListingId",
                table: "PurchaseHistory",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseHistory_UserId",
                table: "PurchaseHistory",
                column: "UserId");
        }
    }
}
