using Microsoft.EntityFrameworkCore.Migrations;

namespace Obskurnee.Migrations
{
    public partial class u2333 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Books_BookId",
                table: "Rounds");

            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Discussions_BookDiscussionId",
                table: "Rounds");

            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Discussions_ThemeDiscussionId",
                table: "Rounds");

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

            migrationBuilder.AlterColumn<int>(
                name: "ThemeTiebreakerPollId",
                table: "Rounds",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "ThemePollId",
                table: "Rounds",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "ThemeDiscussionId",
                table: "Rounds",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "BookTiebreakerPollId",
                table: "Rounds",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "BookPollId",
                table: "Rounds",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "Rounds",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "BookDiscussionId",
                table: "Rounds",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Books_BookId",
                table: "Rounds",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Discussions_BookDiscussionId",
                table: "Rounds",
                column: "BookDiscussionId",
                principalTable: "Discussions",
                principalColumn: "DiscussionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Discussions_ThemeDiscussionId",
                table: "Rounds",
                column: "ThemeDiscussionId",
                principalTable: "Discussions",
                principalColumn: "DiscussionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Polls_BookPollId",
                table: "Rounds",
                column: "BookPollId",
                principalTable: "Polls",
                principalColumn: "PollId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Polls_BookTiebreakerPollId",
                table: "Rounds",
                column: "BookTiebreakerPollId",
                principalTable: "Polls",
                principalColumn: "PollId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Polls_ThemePollId",
                table: "Rounds",
                column: "ThemePollId",
                principalTable: "Polls",
                principalColumn: "PollId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Polls_ThemeTiebreakerPollId",
                table: "Rounds",
                column: "ThemeTiebreakerPollId",
                principalTable: "Polls",
                principalColumn: "PollId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Books_BookId",
                table: "Rounds");

            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Discussions_BookDiscussionId",
                table: "Rounds");

            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Discussions_ThemeDiscussionId",
                table: "Rounds");

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

            migrationBuilder.AlterColumn<int>(
                name: "ThemeTiebreakerPollId",
                table: "Rounds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ThemePollId",
                table: "Rounds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ThemeDiscussionId",
                table: "Rounds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BookTiebreakerPollId",
                table: "Rounds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BookPollId",
                table: "Rounds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "Rounds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BookDiscussionId",
                table: "Rounds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Books_BookId",
                table: "Rounds",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Discussions_BookDiscussionId",
                table: "Rounds",
                column: "BookDiscussionId",
                principalTable: "Discussions",
                principalColumn: "DiscussionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Discussions_ThemeDiscussionId",
                table: "Rounds",
                column: "ThemeDiscussionId",
                principalTable: "Discussions",
                principalColumn: "DiscussionId",
                onDelete: ReferentialAction.Cascade);

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
    }
}
