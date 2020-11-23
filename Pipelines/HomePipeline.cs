using Statiq.Common;
using Statiq.Core;
using Statiq.Razor;
using Statiq.Yaml;

namespace StatiqTutorial
{
    public class HomePipeline : Pipeline
    {
        public HomePipeline()
        {
            InputModules = new ModuleList
            {
                new ReadFiles("content/home.md")
            };

            ProcessModules = new ModuleList {
                new ExtractFrontMatter(new ParseYaml()),
                new MergeContent(new ReadFiles(patterns: "Home.cshtml")),
                new RenderRazor().WithModel(Config.FromDocument((document, context) =>
                {
                    var title = document.GetString("Title");
                    var content = document.GetString("Content");
                    return new HomeViewModel(title, content);
                })),
                new SetDestination(Config.FromDocument((doc, ctx) => new NormalizedPath("index.html")))
            };

            OutputModules = new ModuleList {
                new WriteFiles()
            };
        }
    }
}
