using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFcoreOneToOneBug.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BId",
                table: "A",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BId",
                table: "A");
        }
    }
}
