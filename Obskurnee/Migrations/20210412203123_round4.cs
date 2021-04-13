using Microsoft.EntityFrameworkCore.Migrations;

namespace Obskurnee.Migrations
{
    public partial class round4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookPollPollId",
                table: "Rounds",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookTiebreakerPollPollId",
                table: "Rounds",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_BookPollPollId",
                table: "Rounds",
                column: "BookPollPollId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_BookTiebreakerPollPollId",
                table: "Rounds",
                column: "BookTiebreakerPollPollId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Polls_BookPollPollId",
                table: "Rounds",
                column: "BookPollPollId",
                principalTable: "Polls",
                principalColumn: "PollId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Polls_BookTiebreakerPollPollId",
                table: "Rounds",
                column: "BookTiebreakerPollPollId",
                principalTable: "Polls",
                principalColumn: "PollId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Polls_BookPollPollId",
                table: "Rounds");

            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Polls_BookTiebreakerPollPollId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_BookPollPollId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_BookTiebreakerPollPollId",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "BookPollPollId",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "BookTiebreakerPollPollId",
                table: "Rounds");
        }
    }
}
