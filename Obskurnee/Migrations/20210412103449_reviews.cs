using Microsoft.EntityFrameworkCore.Migrations;

namespace Obskurnee.Migrations
{
    public partial class reviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GoodreadsReview",
                table: "GoodreadsReview");

            migrationBuilder.RenameTable(
                name: "GoodreadsReview",
                newName: "GoodreadsReviews");

            migrationBuilder.AddColumn<int>(
                name: "Kind",
                table: "GoodreadsReviews",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GoodreadsReviews",
                table: "GoodreadsReviews",
                column: "ReviewId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GoodreadsReviews",
                table: "GoodreadsReviews");

            migrationBuilder.DropColumn(
                name: "Kind",
                table: "GoodreadsReviews");

            migrationBuilder.RenameTable(
                name: "GoodreadsReviews",
                newName: "GoodreadsReview");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GoodreadsReview",
                table: "GoodreadsReview",
                column: "ReviewId");
        }
    }
}
