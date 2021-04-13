﻿using LiteDB;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Obskurnee.Models
{
    [Table("Posts")]
    public class Post : HeaderData
    {
        public record OriginalPostReference(Topic topic, int entityId);

        [Key]
        public int PostId { get; set; }

        public int DiscussionId { get; set; }
        public Discussion Discussion { get; set; }

        [NotMapped] //todo
        public OriginalPostReference OriginalPost { get; set; } = null;

        public string Title { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public int PageCount { get; set; }
        public string Url { get; set; }

        [NotMapped]
        [BsonIgnore] public string RenderedText { get => Text.RenderMarkdown(); }

        public string ImageUrl { get; set; }
        public ICollection<Vote> Votes { get; set; }

        public string GetGoodreadsId()
        {
            if (string.IsNullOrWhiteSpace(Url))
            {
                return null;
            }
            var match = Regex.Match(Url, @"goodreads.com\/book\/show\/(\d+).*");
            return match.Groups?[1]?.Value ?? null;
        }

        public Post(string ownerId) : base(ownerId)
        {
        }
    }
}