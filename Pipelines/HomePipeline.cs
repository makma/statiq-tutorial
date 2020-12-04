using Statiq.Common;
using Statiq.Core;
using Statiq.Razor;
using Statiq.Yaml;

namespace StatiqTutorial
{
    /// <summary>
    /// Pipeline responsible for getting and processing home markdown file from the input directory into the Home HTML document using Razor template.
    /// </summary>
    public class HomePipeline : Pipeline
    {
        public HomePipeline()
        {
            InputModules = new ModuleList
            {
                // Reads the content of files from the file system into the content of new documents.
                // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/core/Statiq.Core/Modules/IO/ReadFiles.cs">ReadFiles</see>
                new ReadFiles("content/home.md")
            };

            ProcessModules = new ModuleList {
                // Extracts the first part of the content for each document and sends it to a child module for processing.
                // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/core/Statiq.Core/Modules/Control/ExtractFrontMatter.cs">ExtractFrontMatter</see>
                new ExtractFrontMatter(
                    // Parses YAML content for each input document and stores the result in it's metadata.
                    // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/extensions/Statiq.Yaml/ParseYaml.cs">ParseYaml</see>
                    new ParseYaml()
                ),
                // Loads Razor template to IDocument content.
                // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/core/Statiq.Core/Modules/Content/MergeContent.cs">MergeContent</see>.
                // <see href="https://statiq.dev/web/content-and-data/content/">Content propery of IDocument</see>
                new MergeContent(new ReadFiles(patterns: "Home.cshtml")),

                // Render HTML file from Razor template and document typed as HomeViewModel model.
                // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/extensions/Statiq.Razor/RenderRazor.cs">RenderRazor</see>
                new RenderRazor().WithModel(Config.FromDocument((document, context) =>
                {
                    var title = document.GetString("Title");
                    var content = document.GetString("Content");
                    return new HomeViewModel(title, content);
                })),

                // Set file system destination for the document.
                // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/core/Statiq.Core/Modules/IO/SetDestination.cs">SetDestination</see>
                new SetDestination(Config.FromDocument((doc, ctx) => {
                    // Provides properties and instance methods for working with paths.
                    // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/core/Statiq.Common/IO/NormalizedPath.css">NormalizedPath</see>
                    return new NormalizedPath("index.html");
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
