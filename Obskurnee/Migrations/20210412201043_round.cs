using Microsoft.EntityFrameworkCore.Migrations;

namespace Obskurnee.Migrations
{
    public partial class round : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Round_RoundId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Polls_Round_RoundId",
                table: "Polls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Round",
                table: "Round");

            migrationBuilder.RenameTable(
                name: "Round",
                newName: "Rounds");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rounds",
                table: "Rounds",
                column: "RoundId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Rounds_RoundId",
                table: "Books",
                column: "RoundId",
                principalTable: "Rounds",
                principalColumn: "RoundId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_Rounds_RoundId",
                table: "Polls",
                column: "RoundId",
                principalTable: "Rounds",
                principalColumn: "RoundId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Rounds_RoundId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Polls_Rounds_RoundId",
                table: "Polls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rounds",
                table: "Rounds");

            migrationBuilder.RenameTable(
                name: "Rounds",
                newName: "Round");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Round",
                table: "Round",
                column: "RoundId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Round_RoundId",
                table: "Books",
                column: "RoundId",
                principalTable: "Round",
                principalColumn: "RoundId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_Round_RoundId",
                table: "Polls",
                column: "RoundId",
                principalTable: "Round",
                principalColumn: "RoundId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
