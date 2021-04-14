using Microsoft.EntityFrameworkCore.Migrations;

namespace Obskurnee.Migrations
{
    public partial class poll4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Polls_PollId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_PollId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PollId",
                table: "Posts");

            migrationBuilder.CreateTable(
                name: "PollPost",
                columns: table => new
                {
                    AllPollsPollId = table.Column<int>(type: "INTEGER", nullable: false),
                    OptionsPostId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollPost", x => new { x.AllPollsPollId, x.OptionsPostId });
                    table.ForeignKey(
                        name: "FK_PollPost_Polls_AllPollsPollId",
                        column: x => x.AllPollsPollId,
                        principalTable: "Polls",
                        principalColumn: "PollId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PollPost_Posts_OptionsPostId",
                        column: x => x.OptionsPostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PollPost_OptionsPostId",
                table: "PollPost",
                column: "OptionsPostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PollPost");

            migrationBuilder.AddColumn<int>(
                name: "PollId",
                table: "Posts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PollId",
                table: "Posts",
                column: "PollId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Polls_PollId",
                table: "Posts",
                column: "PollId",
                principalTable: "Polls",
                principalColumn: "PollId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
