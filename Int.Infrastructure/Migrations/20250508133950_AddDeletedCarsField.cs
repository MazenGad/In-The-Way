using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Int.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletedCarsField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeletedCars",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedCars",
                table: "Cars");
        }
    }
}
