using Statiq.Common;
using Statiq.Core;
using Statiq.Markdown;
using Statiq.Yaml;

namespace StatiqTutorial
{
    public class FeatureDetailPipeline : Pipeline
    {
        public FeatureDetailPipeline()
        {

            InputModules = new ModuleList
            {
                new ReadFiles(pattern: "content/features/*.md"),
            };

            ProcessModules = new ModuleList
            {
                new ExtractFrontMatter(new ParseYaml()),
                new RenderMarkdown().UseExtensions(),
                new SetDestination(Config.FromDocument((doc, ctx) =>
                {
                    return new NormalizedPath($"features/{doc.Source.FileNameWithoutExtension}.html");
                }))
            };

            OutputModules = new ModuleList {
                new WriteFiles()
            };
        }
    }
}
