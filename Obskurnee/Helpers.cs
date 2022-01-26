using Markdig;
using Microsoft.Extensions.Localization;
using Obskurnee.Models;
using System.Globalization;
using System.Security.Claims;

namespace Obskurnee;

public static class Helpers
{
    private static readonly MarkdownPipeline _mdPipeline;

    static Helpers()
    {
        var builder = new MarkdownPipelineBuilder();
        builder.Extensions.Insert(0, new SpoilerContainerExtension());
        builder.UseAbbreviations()
            .UseAutoIdentifiers()
            .UseCitations()
            .UseDefinitionLists()
            .UseEmphasisExtras()
            .UseFigures()
            .UseFooters()
            .UseFootnotes()
            .UseGridTables()
            .UseMathematics()
            .UseMediaLinks()
            .UsePipeTables()
            .UseListExtras()
            .UseTaskLists()
            .UseDiagrams()
            .UseAutoLinks()
            .UseGenericAttributes();
        _mdPipeline = builder.Build();
    }

    public static T SetOwner<T>(this T data, ClaimsPrincipal fromUserClaims) where T : HeaderData
    {
        data.OwnerId = fromUserClaims.GetUserId();
        return data;
    }

    public static string GetUserId(this ClaimsPrincipal userClaims) => userClaims.FindFirst(BookclubClaims.UserId)?.Value;

    public static string RenderMarkdown(this string md) => string.IsNullOrWhiteSpace(md) ? "" : Markdown.ToHtml(md, _mdPipeline);

    public static string Format(this IStringLocalizer localizer, string name, params object[] args)
    {
        var origCulture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture = Config.Current.DefaultCultureInfo;
        var str = localizer[name, args];
        CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture = origCulture;
        return str;
    }

    public static string FormatAndRender(this IStringLocalizer localizer, string name, params object[] args)
    {
        var origCulture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture = Config.Current.DefaultCultureInfo;
        var str = localizer[name, args]
            .Value
            .RenderMarkdown();
        CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture = origCulture;
        return str;
    }

    public static string MakeImageRelativePath(string imageFilename)
        => $"/images/{imageFilename}";
}
