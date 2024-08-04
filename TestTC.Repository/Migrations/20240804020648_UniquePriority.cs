using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TC.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UniquePriority : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Priorities_Level",
                table: "Priorities",
                column: "Level",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Priorities_Level",
                table: "Priorities");
        }
    }
}
