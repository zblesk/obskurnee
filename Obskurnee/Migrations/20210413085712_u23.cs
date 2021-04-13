using Microsoft.EntityFrameworkCore.Migrations;

namespace Obskurnee.Migrations
{
    public partial class u23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Polls_BookPollPollId",
                table: "Rounds");

            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Polls_BookTiebreakerPollPollId",
                table: "Rounds");

            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Polls_ThemePollPollId",
                table: "Rounds");

            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Polls_ThemeTiebreakerPollPollId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_BookPollPollId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_BookTiebreakerPollPollId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_ThemePollPollId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_ThemeTiebreakerPollPollId",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "BookPollPollId",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "BookTiebreakerPollPollId",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "ThemePollPollId",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "ThemeTiebreakerPollPollId",
                table: "Rounds");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_BookPollId",
                table: "Rounds",
                column: "BookPollId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_BookTiebreakerPollId",
                table: "Rounds",
                column: "BookTiebreakerPollId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_ThemePollId",
                table: "Rounds",
                column: "ThemePollId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_ThemeTiebreakerPollId",
                table: "Rounds",
                column: "ThemeTiebreakerPollId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Polls_BookPollId",
                table: "Rounds",
                column: "BookPollId",
                principalTable: "Polls",
                principalColumn: "PollId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Polls_BookTiebreakerPollId",
                table: "Rounds",
                column: "BookTiebreakerPollId",
                principalTable: "Polls",
                principalColumn: "PollId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Polls_ThemePollId",
                table: "Rounds",
                column: "ThemePollId",
                principalTable: "Polls",
                principalColumn: "PollId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Polls_ThemeTiebreakerPollId",
                table: "Rounds",
                column: "ThemeTiebreakerPollId",
                principalTable: "Polls",
                principalColumn: "PollId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Polls_BookPollId",
                table: "Rounds");

            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Polls_BookTiebreakerPollId",
                table: "Rounds");

            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Polls_ThemePollId",
                table: "Rounds");

            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Polls_ThemeTiebreakerPollId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_BookPollId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_BookTiebreakerPollId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_ThemePollId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_ThemeTiebreakerPollId",
                table: "Rounds");

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

            migrationBuilder.AddColumn<int>(
                name: "ThemePollPollId",
                table: "Rounds",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThemeTiebreakerPollPollId",
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

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_ThemePollPollId",
                table: "Rounds",
                column: "ThemePollPollId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_ThemeTiebreakerPollPollId",
                table: "Rounds",
                column: "ThemeTiebreakerPollPollId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Polls_ThemePollPollId",
                table: "Rounds",
                column: "ThemePollPollId",
                principalTable: "Polls",
                principalColumn: "PollId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Polls_ThemeTiebreakerPollPollId",
                table: "Rounds",
                column: "ThemeTiebreakerPollPollId",
                principalTable: "Polls",
                principalColumn: "PollId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
