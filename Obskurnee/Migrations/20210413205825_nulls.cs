using Microsoft.EntityFrameworkCore.Migrations;

namespace Obskurnee.Migrations
{
    public partial class nulls : Migration
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
                name: "FK_Books_Posts_PostId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Rounds_RoundId",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "RoundId",
                table: "Books",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Books",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "BookPollId",
                table: "Books",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "BookDiscussionId",
                table: "Books",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

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
                name: "FK_Books_Posts_PostId",
                table: "Books",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Rounds_RoundId",
                table: "Books",
                column: "RoundId",
                principalTable: "Rounds",
                principalColumn: "RoundId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Books_BookDiscussionId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Polls_BookPollId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Posts_PostId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Rounds_RoundId",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "RoundId",
                table: "Books",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Books",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BookPollId",
                table: "Books",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BookDiscussionId",
                table: "Books",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Books_BookDiscussionId",
                table: "Books",
                column: "BookDiscussionId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Polls_BookPollId",
                table: "Books",
                column: "BookPollId",
                principalTable: "Polls",
                principalColumn: "PollId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Posts_PostId",
                table: "Books",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Rounds_RoundId",
                table: "Books",
                column: "RoundId",
                principalTable: "Rounds",
                principalColumn: "RoundId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
