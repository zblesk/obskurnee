﻿using LiteDB;

namespace Obskurnee.Models
{
    public class GoodreadsReview : HeaderData
    {
        public string ReviewId { get; set; }
        public string BookTitle { get; set; }
        public string GoodreadsBookId { get; set; }
        public string ReviewUrl { get; set; }
        public string Author { get; set; }
        public ushort Rating { get; set; }
        public string ReviewText { get; set; }
        [BsonIgnore] public string RenderedReviewText { get => ReviewText.RenderMarkdown(); }
        public string ImageUrl { get; internal set; }

        public GoodreadsReview(string ownerId) : base(ownerId) { }
    }
}