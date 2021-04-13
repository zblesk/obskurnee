using Microsoft.EntityFrameworkCore.Migrations;

namespace Obskurnee.Migrations
{
    public partial class round5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookDiscussionDiscussionId",
                table: "Rounds",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_BookDiscussionDiscussionId",
                table: "Rounds",
                column: "BookDiscussionDiscussionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Discussions_BookDiscussionDiscussionId",
                table: "Rounds",
                column: "BookDiscussionDiscussionId",
                principalTable: "Discussions",
                principalColumn: "DiscussionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Discussions_BookDiscussionDiscussionId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_BookDiscussionDiscussionId",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "BookDiscussionDiscussionId",
                table: "Rounds");
        }
    }
}
