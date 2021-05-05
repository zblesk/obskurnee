using Markdig;
using Markdig.Extensions.CustomContainers;
using Markdig.Parsers.Inlines;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using System.Collections.Generic;

namespace Obskurnee
{
    public class SpoilerContainerRenderer : HtmlObjectRenderer<CustomContainer>
    {
        protected override void Write(HtmlRenderer renderer, CustomContainer obj)
        {
            renderer.EnsureLine();
            if (string.IsNullOrWhiteSpace(obj.Info))
            {
                var attr = obj.GetAttributes();
                attr.AddClass("spoiler");
                obj.SetAttributes(attr);
            }
            if (renderer.EnableHtmlForBlock)
            {
                renderer.Write("<div").WriteAttributes(obj).Write('>');
            }
            // We don't escape a CustomContainer
            renderer.WriteChildren(obj);
            if (renderer.EnableHtmlForBlock)
            {
                renderer.WriteLine("</div>");
            }
        }
    }

    public class SpoilerContainerInlineRenderer : HtmlObjectRenderer<CustomContainerInline>
    {
        protected override void Write(HtmlRenderer renderer, CustomContainerInline obj)
        {
            if (obj != null)
            {
                var attr = obj.TryGetAttributes() ?? new HtmlAttributes { Classes = new List<string>() };
                if (!attr.Classes.Contains("spoiler"))
                {
                    attr.AddClass("spoiler");
                    obj.SetAttributes(attr);
                }
            }
            renderer.Write("<span").WriteAttributes(obj).Write('>');
            renderer.WriteChildren(obj);
            renderer.Write("</span>");
        }
    }

    public class CustomContainerExtension : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.BlockParsers.Contains<CustomContainerParser>())
            {
                // Insert the parser before any other parsers
                pipeline.BlockParsers.Insert(0, new CustomContainerParser());
            }

            // Plug the inline parser for CustomContainerInline
            var inlineParser = pipeline.InlineParsers.Find<EmphasisInlineParser>();
            if (inlineParser != null && !inlineParser.HasEmphasisChar(':'))
            {
                inlineParser.EmphasisDescriptors.Add(new EmphasisDescriptor(':', 2, 2, true));
                inlineParser.TryCreateEmphasisInlineList.Add((emphasisChar, delimiterCount) =>
                {
                    if (delimiterCount == 2 && emphasisChar == ':')
                    {
                        return new CustomContainerInline();
                    }
                    return null;
                });
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is HtmlRenderer htmlRenderer)
            {
                if (!htmlRenderer.ObjectRenderers.Contains<SpoilerContainerRenderer>())
                {
                    // Must be inserted before CodeBlockRenderer
                    htmlRenderer.ObjectRenderers.Insert(0, new SpoilerContainerRenderer());
                }
                if (!htmlRenderer.ObjectRenderers.Contains<SpoilerContainerInlineRenderer>())
                {
                    // Must be inserted before EmphasisRenderer
                    htmlRenderer.ObjectRenderers.Insert(0, new SpoilerContainerInlineRenderer());
                }
            }
        }
    }
}