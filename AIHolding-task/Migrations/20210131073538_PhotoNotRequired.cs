using Microsoft.EntityFrameworkCore.Migrations;

namespace AIHolding_task.Migrations
{
    public partial class PhotoNotRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Files_PhotoID",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "PhotoID",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Files_PhotoID",
                table: "Users",
                column: "PhotoID",
                principalTable: "Files",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Files_PhotoID",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "PhotoID",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Files_PhotoID",
                table: "Users",
                column: "PhotoID",
                principalTable: "Files",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
