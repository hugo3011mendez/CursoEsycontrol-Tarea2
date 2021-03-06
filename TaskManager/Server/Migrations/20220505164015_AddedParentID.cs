using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskManager.Server.Migrations
{
    public partial class AddedParentID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentID",
                table: "Todos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentID",
                table: "Todos");
        }
    }
}
