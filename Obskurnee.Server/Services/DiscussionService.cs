using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Obskurnee.Models;
using Obskurnee.Server;

namespace Obskurnee.Services;

public class DiscussionService
{
    private readonly IStringLocalizer<Strings> _localizer;
    private readonly IStringLocalizer<NewsletterStrings> _newsletterLocalizer;
    private readonly NewsletterService _newsletter;
    private readonly Config _config;
    private readonly ApplicationDbContext _db;

    public DiscussionService(
        ApplicationDbContext db,
        IStringLocalizer<Strings> localizer,
        IStringLocalizer<NewsletterStrings> newsletterLocalizer,
        NewsletterService newsletter,
        Config config)
    {
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        _newsletter = newsletter ?? throw new ArgumentNullException(nameof(newsletter));
        _newsletterLocalizer = newsletterLocalizer ?? throw new ArgumentNullException(nameof(newsletterLocalizer));
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }


    public Task<List<Discussion>> GetAll()
        => (from discussion in _db.Discussions.AsNoTracking()
            orderby discussion.CreatedOn descending
            select discussion)
            .ToListAsync();

    public async Task<Post> NewPost(int discussionId, Post post)
    {
        var discussion = _db.Discussions.AsNoTracking().First(d => d.DiscussionId == discussionId);
        if (discussion.IsClosed)
        {
            throw new Exception(_localizer["discussionClosed"]);
        }
        post.PostId = 0; //ensure it wasn't sent from the client
        post.DiscussionId = discussionId;
        post.Title = post.Title.Trim();
        post.Author = post.Author?.Trim();
        post.Text = post.Text?.Trim();
        await _db.Posts.AddAsync(post);
        await _db.SaveChangesAsync();
        await SendNewPostNotification(discussion, post);
        return post;
    }

    /// <summary>
    /// Get the Post, WITHOUT tracking it in the context
    /// </summary>
    /// <param name="postId"></param>
    /// <returns></returns>
    public Task<Post> GetPost(int postId)
        => _db.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.PostId == postId);

    public async Task<Post> UpdatePost(int discussionId, Post post)
    {
        var existingPost = await _db.Posts.SingleAsync(
            p => p.PostId == post.PostId
                && p.DiscussionId == discussionId);
        existingPost.PageCount = post.PageCount;
        existingPost.Text = post.Text;
        existingPost.Title = post.Title;
        existingPost.Author = post.Author;
        _db.Posts.Update(existingPost);
        await _db.SaveChangesAsync();
        return existingPost;
    }

    private async Task SendNewPostNotification(Discussion discussion, Post post)
    {
        var link = $"{_config.BaseUrl}/navrhy/{discussion.DiscussionId}";
        var body = "";
        if (discussion.Topic == Topic.Books)
        {
            body = _newsletterLocalizer.Format(
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
            body = _newsletterLocalizer.Format(
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
        => await _db.Discussions.AsNoTracking()
        .Include(d => d.Posts).FirstOrDefaultAsync(d => d.DiscussionId == discussionId);

    public async Task<Discussion> GetLatestOpen()
        => await _db.Discussions.AsNoTracking()
        .Where(d => !d.IsClosed).OrderByDescending(d => d.DiscussionId).FirstOrDefaultAsync();
}
