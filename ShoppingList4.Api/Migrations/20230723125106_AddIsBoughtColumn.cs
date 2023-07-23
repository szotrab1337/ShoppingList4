using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingList4.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddIsBoughtColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBought",
                table: "Entries",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBought",
                table: "Entries");
        }
    }
}
