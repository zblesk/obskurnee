using Microsoft.EntityFrameworkCore.Migrations;

namespace Obskurnee.Migrations
{
    public partial class book : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Posts_PostId",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_Round_Book_BookId",
                table: "Round");

            migrationBuilder.DropIndex(
                name: "IX_Round_BookId",
                table: "Round");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Book",
                table: "Book");

            migrationBuilder.RenameTable(
                name: "Book",
                newName: "Books");

            migrationBuilder.RenameIndex(
                name: "IX_Book_PostId",
                table: "Books",
                newName: "IX_Books_PostId");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Books",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoundId",
                table: "Books",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Books",
                table: "Books",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookDiscussionId",
                table: "Books",
                column: "BookDiscussionId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookPollId",
                table: "Books",
                column: "BookPollId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_RoundId",
                table: "Books",
                column: "RoundId",
                unique: true);

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
                name: "FK_Books_Round_RoundId",
                table: "Books",
                column: "RoundId",
                principalTable: "Round",
                principalColumn: "RoundId",
                onDelete: ReferentialAction.Cascade);
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
                name: "FK_Books_Round_RoundId",
                table: "Books");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Books",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookDiscussionId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookPollId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_RoundId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "RoundId",
                table: "Books");

            migrationBuilder.RenameTable(
                name: "Books",
                newName: "Book");

            migrationBuilder.RenameIndex(
                name: "IX_Books_PostId",
                table: "Book",
                newName: "IX_Book_PostId");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Book",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Book",
                table: "Book",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Round_BookId",
                table: "Round",
                column: "BookId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Posts_PostId",
                table: "Book",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Round_Book_BookId",
                table: "Round",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
