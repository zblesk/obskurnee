using Microsoft.EntityFrameworkCore.Migrations;

namespace Obskurnee.Migrations
{
    public partial class poll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Books_BookDiscussionId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Polls_BookPollId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Polls_Rounds_RoundId",
                table: "Polls");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookDiscussionId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookPollId",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "RoundId",
                table: "Polls",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "PreviousPollId",
                table: "Polls",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "DiscussionId",
                table: "Polls",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_Rounds_RoundId",
                table: "Polls",
                column: "RoundId",
                principalTable: "Rounds",
                principalColumn: "RoundId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Polls_Rounds_RoundId",
                table: "Polls");

            migrationBuilder.AlterColumn<int>(
                name: "RoundId",
                table: "Polls",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PreviousPollId",
                table: "Polls",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DiscussionId",
                table: "Polls",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookDiscussionId",
                table: "Books",
                column: "BookDiscussionId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookPollId",
                table: "Books",
                column: "BookPollId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Books_BookDiscussionId",
                table: "Books",
                column: "BookDiscussionId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Polls_BookPollId",
                table: "Books",
                column: "BookPollId",
                principalTable: "Polls",
                principalColumn: "PollId",
                onDelete: ReferentialAction.Restrict);

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
