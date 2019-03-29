using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Platform.Migrations
{
    public partial class videourl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PackageTranslations_Packages_CoreId",
                table: "PackageTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PackageTranslations",
                table: "PackageTranslations");

            migrationBuilder.RenameTable(
                name: "PackageTranslations",
                newName: "PackageTranslations");

            migrationBuilder.RenameIndex(
                name: "IX_PackageTranslations_CoreId",
                table: "PackageTranslations",
                newName: "IX_PackageTranslations_CoreId");

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "StepTranslations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "ProfessionTranslations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "EventTranslations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "BlockTranslations",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "AnswerTranslation",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "AnswerTranslation",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AnswerTranslation",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AnswerTranslation",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "AnswerTranslation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "PackageTranslations",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PackageTranslations",
                table: "PackageTranslations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PackageTranslations_Packages_CoreId",
                table: "PackageTranslations",
                column: "CoreId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PackageTranslations_Packages_CoreId",
                table: "PackageTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PackageTranslations",
                table: "PackageTranslations");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "StepTranslations");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "ProfessionTranslations");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "EventTranslations");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "BlockTranslations");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "AnswerTranslation");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "AnswerTranslation");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AnswerTranslation");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AnswerTranslation");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "AnswerTranslation");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "PackageTranslations");

            migrationBuilder.RenameTable(
                name: "PackageTranslations",
                newName: "PackageTranslations");

            migrationBuilder.RenameIndex(
                name: "IX_PackageTranslations_CoreId",
                table: "PackageTranslations",
                newName: "IX_PackageTranslations_CoreId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PackageTranslations",
                table: "PackageTranslations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PackageTranslations_Packages_CoreId",
                table: "PackageTranslations",
                column: "CoreId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
