using System.Collections.Generic;
using Statiq.Common;
using Statiq.Core;
using Statiq.Razor;
using Statiq.Yaml;

namespace StatiqTutorial
{
    public class FeaturesListingRazorPipeline : Pipeline
    {
        public FeaturesListingRazorPipeline()
        {
            InputModules = new ModuleList
            {
                new ReadFiles(pattern: "content/features/*.md"),
            };

            ProcessModules = new ModuleList
            {
                new ExtractFrontMatter(new ParseYaml()),
                new ReplaceDocuments(
                    Config.FromContext(context =>
                    {
                        return (IEnumerable<IDocument>) new []
                        {
                            context.CreateDocument(
                                new NormalizedPath("features-razor.html"),
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
                new MergeContent(new ReadFiles("FeaturesListing.cshtml")),
                new RenderRazor().WithModel(Config.FromDocument((document, context) =>
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
                new WriteFiles()
            };
        }
    }
}
