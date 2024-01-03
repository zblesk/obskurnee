using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Obskurnee.Migrations
{
    public partial class BasicsupportforStorygraph : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExternalSystem",
                table: "GoodreadsReviews",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Series",
                table: "GoodreadsReviews",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExternalProfileSystem",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalSystem",
                table: "GoodreadsReviews");

            migrationBuilder.DropColumn(
                name: "Series",
                table: "GoodreadsReviews");

            migrationBuilder.DropColumn(
                name: "ExternalProfileSystem",
                table: "AspNetUsers");
        }
    }
}
