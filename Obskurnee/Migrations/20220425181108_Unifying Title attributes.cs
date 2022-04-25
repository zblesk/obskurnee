using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Obskurnee.Migrations
{
    public partial class UnifyingTitleattributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookTitle",
                table: "GoodreadsReviews",
                newName: "Title");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "GoodreadsReviews",
                newName: "BookTitle");
        }
    }
}
