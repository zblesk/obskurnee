using Microsoft.EntityFrameworkCore.Migrations;

namespace Obskurnee.Migrations
{
    public partial class polls3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SerializedPostIds",
                table: "Votes",
                newName: "PostIdsSerialized");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostIdsSerialized",
                table: "Votes",
                newName: "SerializedPostIds");
        }
    }
}
