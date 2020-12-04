using Statiq.Common;
using Statiq.Core;
using Statiq.Markdown;
using Statiq.Yaml;

namespace StatiqTutorial
{
    /// <summary>
    /// Pipeline responsible for processing features markdown files into feature deatail HTML documents using default render markdown module.
    /// </summary>
    public class FeatureDetailPipeline : Pipeline
    {
        public FeatureDetailPipeline()
        {

            InputModules = new ModuleList
            {
                // Reads the content of files from the file system into the content of new documents.
                // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/core/Statiq.Core/Modules/IO/ReadFiles.cs">ReadFiles</see>
                new ReadFiles(pattern: "content/features/*.md"),
            };

            ProcessModules = new ModuleList
            {
                // Extracts the first part of the content for each document and sends it to a child module for processing.
                // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/core/Statiq.Core/Modules/Control/ExtractFrontMatter.cs">ExtractFrontMatter</see>
                new ExtractFrontMatter(
                    // Parses YAML content for each input document and stores the result in it's metadata.
                    // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/extensions/Statiq.Yaml/ParseYaml.cs">ParseYaml</see>
                    new ParseYaml()
                ),

                // Parses markdown content and renders it to HTML.
                // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/extensions/Statiq.Markdown/RenderMarkdown.cs">RenderMarkdown</see>
                new RenderMarkdown().UseExtensions(),

                // Set file system destination for the document.
                // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/core/Statiq.Core/Modules/IO/SetDestination.cs">SetDestination</see>
                new SetDestination(Config.FromDocument((doc, ctx) =>
                {
                    // Provides properties and instance methods for working with paths.
                    // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/core/Statiq.Common/IO/NormalizedPath.css">NormalizedPath</see>
                    return new NormalizedPath($"features/{doc.Source.FileNameWithoutExtension}.html");
                }))
            };

            OutputModules = new ModuleList {
                // Flush to the output.
                // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/core/Statiq.Core/Modules/IO/WriteFiles.cs">WriteFiles</see>.
                new WriteFiles()
            };
        }
    }
}
