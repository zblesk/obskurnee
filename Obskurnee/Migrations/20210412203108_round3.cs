using Microsoft.EntityFrameworkCore.Migrations;

namespace Obskurnee.Migrations
{
    public partial class round3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ThemeTiebreakerPollPollId",
                table: "Rounds",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_ThemeTiebreakerPollPollId",
                table: "Rounds",
                column: "ThemeTiebreakerPollPollId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Polls_ThemeTiebreakerPollPollId",
                table: "Rounds",
                column: "ThemeTiebreakerPollPollId",
                principalTable: "Polls",
                principalColumn: "PollId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Polls_ThemeTiebreakerPollPollId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_ThemeTiebreakerPollPollId",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "ThemeTiebreakerPollPollId",
                table: "Rounds");
        }
    }
}
