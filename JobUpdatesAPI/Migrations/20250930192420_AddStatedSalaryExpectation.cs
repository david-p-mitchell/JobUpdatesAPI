using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobUpdatesAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddStatedSalaryExpectation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatedSalaryExpectation",
                table: "Jobs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatedSalaryExpectation",
                table: "Jobs");
        }
    }
}
