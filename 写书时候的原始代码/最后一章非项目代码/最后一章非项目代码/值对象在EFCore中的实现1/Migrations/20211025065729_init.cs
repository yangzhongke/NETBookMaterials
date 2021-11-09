using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace 值对象在EFCore中的实现1.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Chinese = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name_English = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Area_Value = table.Column<double>(type: "float", nullable: false),
                    Area_Unit = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Population = table.Column<long>(type: "bigint", nullable: true),
                    Location_Longitude = table.Column<double>(type: "float", nullable: false),
                    Location_Latitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
