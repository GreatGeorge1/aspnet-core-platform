using Microsoft.EntityFrameworkCore.Migrations;

namespace Platform.Migrations
{
    public partial class steps2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Discriminator",
                table: "Steps",
                newName: "StepType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StepType",
                table: "Steps",
                newName: "Discriminator");
        }
    }
}
