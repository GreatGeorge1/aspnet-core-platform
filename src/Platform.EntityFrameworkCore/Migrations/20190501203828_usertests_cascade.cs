using Microsoft.EntityFrameworkCore.Migrations;

namespace Platform.Migrations
{
    public partial class usertests_cascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTests_UserProfessions_UserProfessionId",
                table: "UserTests");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTests_UserProfessions_UserProfessionId",
                table: "UserTests",
                column: "UserProfessionId",
                principalTable: "UserProfessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTests_UserProfessions_UserProfessionId",
                table: "UserTests");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTests_UserProfessions_UserProfessionId",
                table: "UserTests",
                column: "UserProfessionId",
                principalTable: "UserProfessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
