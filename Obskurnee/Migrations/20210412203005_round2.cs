using Microsoft.EntityFrameworkCore.Migrations;

namespace Obskurnee.Migrations
{
    public partial class round2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Polls_Rounds_RoundId",
                table: "Polls");

            migrationBuilder.DropIndex(
                name: "IX_Polls_RoundId",
                table: "Polls");

            migrationBuilder.AddColumn<int>(
                name: "ThemeDiscussionDiscussionId",
                table: "Rounds",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThemePollPollId",
                table: "Rounds",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_ThemeDiscussionDiscussionId",
                table: "Rounds",
                column: "ThemeDiscussionDiscussionId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_ThemePollPollId",
                table: "Rounds",
                column: "ThemePollPollId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Discussions_ThemeDiscussionDiscussionId",
                table: "Rounds",
                column: "ThemeDiscussionDiscussionId",
                principalTable: "Discussions",
                principalColumn: "DiscussionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Polls_ThemePollPollId",
                table: "Rounds",
                column: "ThemePollPollId",
                principalTable: "Polls",
                principalColumn: "PollId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Discussions_ThemeDiscussionDiscussionId",
                table: "Rounds");

            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Polls_ThemePollPollId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_ThemeDiscussionDiscussionId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_ThemePollPollId",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "ThemeDiscussionDiscussionId",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "ThemePollPollId",
                table: "Rounds");

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
    }
}
