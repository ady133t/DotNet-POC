using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace My_Dashboard.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Test",
                newName: "Name1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name1",
                table: "Test",
                newName: "Name");
        }
    }
}
