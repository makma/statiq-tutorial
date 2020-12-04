using System.Collections.Generic;
using Statiq.Common;
using Statiq.Core;
using Statiq.Handlebars;
using Statiq.Yaml;

namespace StatiqTutorial
{
    /// <summary>
    /// Pipeline responsible for processing features markdown files into features listing HTML documents using handlebars templates.
    /// </summary>
    public class FeatureListingHandlebarsPipeline : Pipeline
    {
        public FeatureListingHandlebarsPipeline()
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

                // Replaces documents in the current pipeline.
                // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/core/Statiq.Core/Modules/Control/ReplaceDocuments.cs">ReplaceDocuments</see>
                new ReplaceDocuments(
                    Config.FromContext(context =>
                    {
                        return (IEnumerable<IDocument>) new []
                        {
                            context.CreateDocument(
                                // Provides properties and instance methods for working with paths.
                                // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/core/Statiq.Common/IO/NormalizedPath.css">NormalizedPath</see>
                                new NormalizedPath("features-handlebars.html"),
                                new[]
                                {
                                    new KeyValuePair<string, object>(Keys.Children, context.Inputs)
                                }
                            )
                        };
                    })
                ),
            };

            OutputModules = new ModuleList
            {
                // Load HandleBars template to IDocument content.
                // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/core/Statiq.Core/Modules/Content/MergeContent.cs">MergeContent</see>.
                // <see href="https://statiq.dev/web/content-and-data/content/">Content propery of IDocument</see>
                new MergeContent(
                    // Reads the content of files from the file system into the content of new documents.
                    // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/core/Statiq.Core/Modules/IO/ReadFiles.cs">ReadFiles</see>
                    new ReadFiles("FeaturesListing.hbs")
                    ),

                // Parses, compiles, and renders Handlebars templates.
                // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/extensions/Statiq.Handlebars/RenderHandlebars.cs">RenderHandlebars</see>
                new RenderHandlebars().WithModel(Config.FromDocument((document, context) =>
                {
                    var featureDocuments = document.GetChildren();
                    List<Feature> features = new List<Feature>();

                    foreach (var featureDocument in featureDocuments)
                    {
                        var featureTitle = featureDocument.GetString("Title");
                        var featureDescription = featureDocument.GetString("Description");
                        var slug = featureDocument.Source.FileNameWithoutExtension.ToString();
                        features.Add(new Feature(featureTitle, featureDescription, slug));
                    }
                    return new FeaturesListingViewModel(features);
                })),
                // Flush to the output.
                // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/core/Statiq.Core/Modules/IO/WriteFiles.cs">WriteFiles</see>.
                new WriteFiles()
            };
        }
    }
}
