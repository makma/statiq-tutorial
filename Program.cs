using System.Threading.Tasks;
using Statiq.App;
using Statiq.Web;

namespace StatiqTutorial
{
  public class Program
  {
    public static async Task<int> Main(string[] args) =>
      await Bootstrapper
        .Factory
        .CreateDefault(args) // Creates a bootstrapper with a default configuration including logging, commands, shortcodes, and assembly scanning. Disables automatic creating of html pages from markdonw files. Fixes https://github.com/statiqdev/Statiq.Framework/issues/140
        .AddHostingCommands() // enables preview pipeline
        .RunAsync();
  }
}
