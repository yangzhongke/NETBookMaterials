using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace 多对多配置.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_Students",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_Teachers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Teachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_Students_Teachers",
                columns: table => new
                {
                    StudentsId = table.Column<long>(type: "bigint", nullable: false),
                    TeachersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Students_Teachers", x => new { x.StudentsId, x.TeachersId });
                    table.ForeignKey(
                        name: "FK_T_Students_Teachers_T_Students_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "T_Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_Students_Teachers_T_Teachers_TeachersId",
                        column: x => x.TeachersId,
                        principalTable: "T_Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_Students_Teachers_TeachersId",
                table: "T_Students_Teachers",
                column: "TeachersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_Students_Teachers");

            migrationBuilder.DropTable(
                name: "T_Students");

            migrationBuilder.DropTable(
                name: "T_Teachers");
        }
    }
}
