using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaEncoder.Infrastructure.Migrations
{
    public partial class Add_Fields_SourceSystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SourceSystem",
                table: "T_ME_EncodingItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceSystem",
                table: "T_ME_EncodingItems");
        }
    }
}
