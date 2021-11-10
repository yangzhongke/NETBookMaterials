using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace 第一个EFCore项目.Migrations
{
    public partial class AddAuthorName_ModifyTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "T_Books",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "T_Books",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "T_Books");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "T_Books",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
