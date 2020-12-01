using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Delivery.Extensions;
using Statiq.App;
using Statiq.Common;
using Statiq.Web;

namespace StatiqTutorial
{
    public class Program
    {
        public static async Task<int> Main(string[] args) =>
          await Bootstrapper
            .Factory
            .CreateDefault(args) // Creates a bootstrapper with a default configuration including logging, commands, shortcodes, and assembly scanning. Disables automatic creating of html pages from markdonw files. Fixes https://github.com/statiqdev/Statiq.Framework/issues/140
            .ConfigureServices((services, settings) =>
            {
                DeliveryOptions deliveryOptions = new DeliveryOptions();
                deliveryOptions.ProjectId = settings.GetString("ProjectId");
                deliveryOptions.PreviewApiKey = !string.IsNullOrEmpty(settings.GetString("PreviewApiKey")) ? settings.GetString("PreviewApiKey") : null;
                deliveryOptions.UsePreviewApi = settings.GetBool("UsePreviewApi");

                services.AddSingleton<ITypeProvider, CustomTypeProvider>();
                services.AddDeliveryClient(deliveryOptions);
            })
        .AddHostingCommands() // enables preview pipeline
        .RunAsync();
    }
}
