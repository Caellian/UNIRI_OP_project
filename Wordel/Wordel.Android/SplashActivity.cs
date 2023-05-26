using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Application = Android.App.Application;
using Avalonia;
using Avalonia.Android;
using Avalonia.ReactiveUI;
using Wordel.Util;

namespace Wordel.Android;

[Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
public class SplashActivity : AvaloniaSplashActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        Io.Environment = RunEnv.Android;
        
        return base.CustomizeAppBuilder(builder)
            .UseReactiveUI();
    }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
    }

    protected override void OnResume()
    {
        base.OnResume();

        try
        {
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
        catch (Exception e)
        {
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "wordel-crash.txt");
            using (var writer = File.CreateText(backingFile))
            {
                writer.Write(e);
            }
        }
    }
}