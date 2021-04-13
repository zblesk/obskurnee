using Microsoft.EntityFrameworkCore.Migrations;

namespace Obskurnee.Migrations
{
    public partial class u233 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Books_RoundId",
                table: "Books");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_BookId",
                table: "Rounds",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_RoundId",
                table: "Books",
                column: "RoundId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Books_BookId",
                table: "Rounds",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Books_BookId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_BookId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Books_RoundId",
                table: "Books");

            migrationBuilder.CreateIndex(
                name: "IX_Books_RoundId",
                table: "Books",
                column: "RoundId",
                unique: true);
        }
    }
}
