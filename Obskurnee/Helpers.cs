using Markdig;
using Microsoft.Extensions.Localization;
using Obskurnee.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Obskurnee
{
    public static class Helpers
    {
        private static MarkdownPipeline _mdPipeline;

        static Helpers()
        {
            _mdPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        }

        public static T SetOwner<T>(this T data, ClaimsPrincipal fromUserClaims)  where T : HeaderData
        {
            data.OwnerId = fromUserClaims.GetUserId();
            return data;
        }

        public static string GetUserId(this ClaimsPrincipal userClaims) => userClaims.FindFirst(BookclubClaims.UserId).Value;
        
        public static string RenderMarkdown(this string md) => string.IsNullOrWhiteSpace(md) ? "" : Markdown.ToHtml(md, _mdPipeline);

        public static string AddMarkdownQuote(this string text) => text.Replace("\n", "\n> ");

        public static string Format(this IStringLocalizer localizer, string name, params object[] args)
            => string.Format(localizer[name], args);

        public static string FormatAndRender(this IStringLocalizer localizer, string name, params object[] args)
            => localizer.Format(name, args)
                .RenderMarkdown();

        public static Dictionary<string, dynamic> ToWritableDictionary(this object dict)
            => JsonSerializer.Deserialize<Dictionary<string, dynamic>>(JsonSerializer.Serialize(dict));

        public static string RemoveDiacritics(this string text)
        {
            return string.Concat(
                text.Normalize(NormalizationForm.FormD)
                .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) !=
                                              UnicodeCategory.NonSpacingMark)
              ).Normalize(NormalizationForm.FormC);
        }
    }
}
