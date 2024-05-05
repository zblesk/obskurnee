using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Obskurnee.Migrations
{
    /// <inheritdoc />
    public partial class AddingSearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE VIRTUAL TABLE FtsPosts USING fts5(PostId, Title, Author, OwnerId, Text, Kind, HasParent);
");

            // migrate existing posts
            migrationBuilder.Sql(@"
INSERT INTO FtsPosts (PostId, Title, Author, OwnerId, Text, Kind, HasParent)
SELECT PostId, Posts.Title, Author, Posts.OwnerId, Text, 'Post', (ParentPostId is not null or ParentRecommendationId is not null) HasParent 
FROM Posts
JOIN Discussions on Posts.DiscussionId = Discussions.DiscussionId
WHERE Discussions.Topic = 1");
            migrationBuilder.Sql(@"
INSERT INTO FtsPosts (PostId, Title, Author, OwnerId, Text, Kind, HasParent)
SELECT RecommendationId, Title, Author, OwnerId, Text, 'Rec', 0
FROM Recommendations;");

            migrationBuilder.Sql(@"
CREATE TRIGGER UpdatePostSearch 
  AFTER UPDATE ON Posts
BEGIN
  UPDATE FtsPosts
  SET 
	Text = NEW.Text,
	Title = NEW.Title,
	Author = NEW.Author, 
	OwnerId = NEW.OwnerId
  WHERE
	PostId = NEW.PostId
	and Kind = 'Post';
END;

CREATE TRIGGER CreatePostSearch 
  AFTER INSERT ON Posts
  WHEN EXISTS (SELECT 1 FROM Discussions WHERE DiscussionId = NEW.DiscussionId AND Discussions.Topic = 1)
BEGIN
  INSERT INTO FtsPosts 
  (PostId, Title, Author, OwnerId, Text, Kind, HasParent)
  VALUES (
    NEW.PostId,
	NEW.Title,
	NEW.Author, 
	NEW.OwnerId,
	NEW.Text,
	'Post',
	(NEW.ParentPostId is not null or NEW.ParentRecommendationId is not null));
END;

CREATE TRIGGER DeletePostSearch 
  AFTER DELETE ON Posts
BEGIN
  DELETE FROM FtsPosts
  WHERE
	PostId = OLD.PostId
	and Kind = 'Post';
END;");

            migrationBuilder.Sql(@"
CREATE TRIGGER UpdateRecommendationSearch 
  AFTER UPDATE ON Recommendations
BEGIN
  UPDATE FtsPosts
  SET 
	Text = NEW.Text,
	Title = NEW.Title,
	Author = NEW.Author, 
	OwnerId = NEW.OwnerId
  WHERE
	PostId = NEW.RecommendationId
	and Kind = 'Rec';
END;

CREATE TRIGGER CreateRecommendationSearch 
  AFTER INSERT ON Recommendations
BEGIN
  INSERT INTO FtsPosts 
  (PostId, Title, Author, OwnerId, Text, Kind, HasParent)
  VALUES (
    NEW.RecommendationId,
	NEW.Title,
	NEW.Author, 
	NEW.OwnerId,
	NEW.Text,
	'Rec',
	0);
END;

CREATE TRIGGER DeleteRecommendationSearch 
  AFTER DELETE ON Recommendations
BEGIN
  DELETE FROM FtsPosts
  WHERE
	PostId = OLD.RecommendationId
	and Kind = 'Rec';
END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DROP TABLE FtsPosts;
");
        }
    }
}
