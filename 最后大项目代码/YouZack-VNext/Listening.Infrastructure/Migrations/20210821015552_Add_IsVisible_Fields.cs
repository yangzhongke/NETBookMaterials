using Microsoft.EntityFrameworkCore.Migrations;

namespace Listening.Infrastructure.Migrations
{
    public partial class Add_IsVisible_Fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AudioDuration",
                table: "T_Episodes");

            migrationBuilder.DropColumn(
                name: "AutoIncId",
                table: "T_Albums");

            migrationBuilder.AddColumn<double>(
                name: "DurationInSecond",
                table: "T_Episodes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "T_Episodes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "T_Albums",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationInSecond",
                table: "T_Episodes");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "T_Episodes");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "T_Albums");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "AudioDuration",
                table: "T_Episodes",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<long>(
                name: "AutoIncId",
                table: "T_Albums",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");
        }
    }
}
