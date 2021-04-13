using Microsoft.EntityFrameworkCore.Migrations;

namespace Obskurnee.Migrations
{
    public partial class polls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Books_BookId",
                table: "Rounds");

            migrationBuilder.DropTable(
                name: "PostVote");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_BookId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Books_RoundId",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Votes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerializedPostIds",
                table: "Votes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FollowupLinkSerialized",
                table: "Polls",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResultsSerialized",
                table: "Polls",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Votes_OwnerId",
                table: "Votes",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_PostId",
                table: "Votes",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_RoundId",
                table: "Books",
                column: "RoundId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Posts_PostId",
                table: "Votes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Posts_PostId",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Votes_OwnerId",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Votes_PostId",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Books_RoundId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "SerializedPostIds",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "FollowupLinkSerialized",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "ResultsSerialized",
                table: "Polls");

            migrationBuilder.CreateTable(
                name: "PostVote",
                columns: table => new
                {
                    PostsPostId = table.Column<int>(type: "INTEGER", nullable: false),
                    VotesVoteId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostVote", x => new { x.PostsPostId, x.VotesVoteId });
                    table.ForeignKey(
                        name: "FK_PostVote_Posts_PostsPostId",
                        column: x => x.PostsPostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostVote_Votes_VotesVoteId",
                        column: x => x.VotesVoteId,
                        principalTable: "Votes",
                        principalColumn: "VoteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_BookId",
                table: "Rounds",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_RoundId",
                table: "Books",
                column: "RoundId");

            migrationBuilder.CreateIndex(
                name: "IX_PostVote_VotesVoteId",
                table: "PostVote",
                column: "VotesVoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Books_BookId",
                table: "Rounds",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
