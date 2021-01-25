using Markdig;
using Obskurnee.Models;
using System.Security.Claims;

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
            data.OwnerId = fromUserClaims.FindFirst(ClaimTypes.NameIdentifier).Value;
            return data;
        }

        public static string GetUserId(this ClaimsPrincipal userClaims) => userClaims.FindFirst(ClaimTypes.NameIdentifier).Value;
        
        public static string RenderMarkdown(this string md) => string.IsNullOrWhiteSpace(md) ? "" : Markdown.ToHtml(md, _mdPipeline);
    }
}
