using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityDemo1.Migrations
{
    public partial class Add_JWTVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "JWTVersion",
                table: "AspNetUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JWTVersion",
                table: "AspNetUsers");
        }
    }
}
