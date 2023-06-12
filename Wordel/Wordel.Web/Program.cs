using System.Runtime.Versioning;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Browser;
using Avalonia.ReactiveUI;
using Wordel;
using Wordel.Util;

[assembly: SupportedOSPlatform("browser")]
internal partial class Program
{
    private static async Task Main(string[] args)
    {
        Wordel.Util.SysUtil.Environment = RunEnv.Web;
        
        await BuildAvaloniaApp()
            .UseReactiveUI()
            .StartBrowserAppAsync("out");
    }
    
    public static AppBuilder BuildAvaloniaApp()
    {
        Wordel.Util.SysUtil.Environment = RunEnv.Android;
        return AppBuilder.Configure<App>();
    }
}