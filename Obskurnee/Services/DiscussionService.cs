using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Services
{
    public class DiscussionService
    {
        private readonly ILogger<DiscussionService> _logger;
        private readonly IStringLocalizer<Strings> _localizer;
        private readonly IStringLocalizer<NewsletterStrings> _newsletterLocalizer;
        private readonly NewsletterService _newsletter;
        private readonly Config _config;
        private readonly ApplicationDbContext _db;

        public DiscussionService(
            ILogger<DiscussionService> logger,
            ApplicationDbContext db,
            IStringLocalizer<Strings> localizer,
            IStringLocalizer<NewsletterStrings> newsletterLocalizer,
            NewsletterService newsletter,
            Config config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _newsletter = newsletter ?? throw new ArgumentNullException(nameof(newsletter));
            _newsletterLocalizer = newsletterLocalizer ?? throw new ArgumentNullException(nameof(newsletterLocalizer));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }


        public Task<List<Discussion>> GetAll()
            => (from discussion in _db.Discussions
                orderby discussion.CreatedOn descending
                select discussion)
                .ToListAsync();

        public async Task<Post> NewPost(int discussionId, Post post)
        {
            var discussion = _db.Discussions.First(d => d.DiscussionId == discussionId);
            if (discussion.IsClosed)
            {
                throw new Exception(_localizer["discussionClosed"]);
            }
            post.PostId = 0; //ensure it wasn't sent from the client
            post.DiscussionId = discussionId;
            post.Title = post.Title.Trim();
            post.Author = post.Author.Trim();
            post.Text = post.Text.Trim();
            await _db.Posts.AddAsync(post);
            await _db.SaveChangesAsync();
            await SendNewPostNotification(discussion, post);
            return post;
        }

        private async Task SendNewPostNotification(Discussion discussion, Post post)
        {
            var link = $"{_config.BaseUrl}/navrhy/{discussion.DiscussionId}";
            var body = "";
            if (discussion.Topic == Topic.Books)
            {
                body = _newsletterLocalizer.FormatAndRender(
                    "newBookPostBodyMarkdown",
                    post.Title,
                    post.Author,
                    post.Text.AddMarkdownQuote(),
                    link,
                    post.PageCount,
                    post.ImageUrl,
                    post.Url);
            }
            else if (discussion.Topic == Topic.Themes)
            {
                body = _newsletterLocalizer.FormatAndRender(
                    "newThemePostBodyMarkdown",
                    post.Title,
                    post.Text,
                    link);
            }
            await _newsletter.SendNewsletter(
                Newsletters.AllEvents,
                _newsletterLocalizer.Format("newPostSubject", post.OwnerName),
                body);
        }

        public async Task<Discussion> GetWithPosts(int discussionId) 
            => await _db.Discussions.Include(d => d.Posts).FirstOrDefaultAsync(d => d.DiscussionId == discussionId);

        public async Task<Discussion> GetLatestOpen()
            => await _db.Discussions.Where(d => !d.IsClosed).OrderByDescending(d => d.DiscussionId).FirstOrDefaultAsync();
    }
}
