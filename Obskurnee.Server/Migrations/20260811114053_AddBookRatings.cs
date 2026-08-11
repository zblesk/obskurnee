using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Obskurnee.Migrations
{
    /// <inheritdoc />
    public partial class AddBookRatings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Rating",
                table: "Books",
                type: "REAL",
                nullable: true);

            // calculate for existing books
            migrationBuilder.Sql(@"
UPDATE Books
SET Rating = avg_ratings.AvgRating
FROM (
    SELECT bcr.BookId, AVG(bcr.Rating) AS AvgRating
    FROM BookclubReviews bcr
    WHERE bcr.Rating IS NOT NULL
    GROUP BY bcr.BookId
) AS avg_ratings
WHERE avg_ratings.BookId = Books.BookId;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Books");
        }
    }
}
