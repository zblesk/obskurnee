using Microsoft.EntityFrameworkCore.Migrations;

namespace Obskurnee.Migrations
{
    public partial class u2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Discussions_ThemeDiscussionDiscussionId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_ThemeDiscussionDiscussionId",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "ThemeDiscussionDiscussionId",
                table: "Rounds");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_ThemeDiscussionId",
                table: "Rounds",
                column: "ThemeDiscussionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Discussions_ThemeDiscussionId",
                table: "Rounds",
                column: "ThemeDiscussionId",
                principalTable: "Discussions",
                principalColumn: "DiscussionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Discussions_ThemeDiscussionId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_ThemeDiscussionId",
                table: "Rounds");

            migrationBuilder.AddColumn<int>(
                name: "ThemeDiscussionDiscussionId",
                table: "Rounds",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_ThemeDiscussionDiscussionId",
                table: "Rounds",
                column: "ThemeDiscussionDiscussionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Discussions_ThemeDiscussionDiscussionId",
                table: "Rounds",
                column: "ThemeDiscussionDiscussionId",
                principalTable: "Discussions",
                principalColumn: "DiscussionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
