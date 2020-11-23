using Statiq.Common;
using Statiq.Core;
using Statiq.Handlebars;
using Statiq.Yaml;
using System.Collections.Generic;

namespace StatiqTutorial
{
    public class FeatureListingHandlebarsPipeline : Pipeline
    {
        public FeatureListingHandlebarsPipeline()
        {
            InputModules = new ModuleList
            {
                new ReadFiles(pattern: "content/features/*.md"),
            };

            ProcessModules = new ModuleList {
                new ExtractFrontMatter(new ParseYaml()),
                new MergeContent(new ReadFiles(patterns: "FeaturesListing.hbs")),
                new RenderHandlebars().WithModel(Config.FromContext((context) =>
                {
                    List<Feature> features = new List<Feature>();
                    foreach (var featureDocument in context.Inputs)
                    {
                        var featureTitle = featureDocument.GetString("Title");
                        var featureDescription = featureDocument.GetString("Description");
                        var slug = featureDocument.Source.FileNameWithoutExtension.ToString();
                        features.Add(new Feature(featureTitle, featureDescription, slug));
                    }
                    return new FeaturesListingViewModel(features);
                })),
                new SetDestination(Config.FromDocument((doc, ctx) => new NormalizedPath("features-handlebars.html")))
            };

            OutputModules = new ModuleList {
                new WriteFiles()
            };
        }
    }
}
