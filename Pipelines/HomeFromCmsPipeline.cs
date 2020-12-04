using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Delivery.Urls.QueryParameters;
using Kentico.Kontent.Delivery.Urls.QueryParameters.Filters;
using Kontent.Statiq;
using Statiq.Common;
using Statiq.Core;
using Statiq.Razor;

namespace StatiqTutorial
{
    /// <summary>
    /// Pipeline responsible for getting and processing Home content item from headless CMS Kontent into home HTML document using Razor template.
    /// </summary>
    public class HomeFromCmsPipeline : Pipeline
    {
        public HomeFromCmsPipeline(IDeliveryClient client)
        {
            InputModules = new ModuleList
            {
                // Load the "Home" item and transfer it into IDocument.
                // <see href="https://github.com/alanta/Kontent.Statiq">Kontent.Statiq</see>
                new Kontent<Home>(client).WithQuery(
                    new EqualsFilter("system.codename", "hello_world_from_statiq_"),
                    new LimitParameter(1)),

                // Set file system destination for the document.
                // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/core/Statiq.Core/Modules/IO/SetDestination.cs">SetDestination</see>
                new SetDestination(
                    // Provides properties and instance methods for working with paths.
                    // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/core/Statiq.Common/IO/NormalizedPath.css">NormalizedPath</see>
                    new NormalizedPath("index-from-cms.html")
                )
            };

            ProcessModules = new ModuleList {
                // Load Razor template to IDocument content.
                // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/core/Statiq.Core/Modules/Content/MergeContent.cs">MergeContent</see>.
                // <see href="https://statiq.dev/web/content-and-data/content/">Content propery of IDocument</see>
                new MergeContent(
                    // Reads the content of files from the file system into the content of new documents.
                    // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/core/Statiq.Core/Modules/IO/ReadFiles.cs">ReadFiles</see>
                    new ReadFiles(patterns: "Home.cshtml")
                    ),

                // Render HTML file from Razor template and document typed as HomeViewModel model.
                // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/extensions/Statiq.Razor/RenderRazor.cs">RenderRazor</see>
                new RenderRazor().WithModel(Config.FromDocument((document, context) =>
                    new HomeViewModel(document.AsKontent<Home>()
                ))),
            };

            OutputModules = new ModuleList {
                // Flush to the output.
                // <see href="https://github.com/statiqdev/Statiq.Framework/blob/main/src/core/Statiq.Core/Modules/IO/WriteFiles.cs">WriteFiles</see>.
                new WriteFiles(),
            };
        }
    }
}