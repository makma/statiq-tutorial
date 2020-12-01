using Kentico.Kontent.Delivery.Abstractions;
using Kontent.Statiq;
using Statiq.Common;
using Statiq.Core;
using Statiq.Razor;

namespace StatiqTutorial
{
    public class HomeFromCmsPipeline : Pipeline
    {
        public HomeFromCmsPipeline(IDeliveryClient client)
        {
            InputModules = new ModuleList
            {
                new Kontent<Home>(client),
                new SetDestination(new NormalizedPath("index-from-cms.html" ))
            };

            ProcessModules = new ModuleList {
                new MergeContent(new ReadFiles(patterns: "Home.cshtml")),
                new RenderRazor().WithModel(Config.FromDocument((document, context) =>
                    new HomeViewModel(document.AsKontent<Home>()
                ))),
                new KontentImageProcessor()
            };

            OutputModules = new ModuleList {
                new WriteFiles(),
            };
        }
    }
}