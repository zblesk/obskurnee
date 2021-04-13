using Microsoft.EntityFrameworkCore.Migrations;

namespace Obskurnee.Migrations
{
    public partial class polls4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Polls_RoundId",
                table: "Polls",
                column: "RoundId");

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
                name: "FK_Polls_Rounds_RoundId",
                table: "Polls");

            migrationBuilder.DropIndex(
                name: "IX_Polls_RoundId",
                table: "Polls");
        }
    }
}
