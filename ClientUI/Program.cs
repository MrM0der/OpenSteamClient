﻿using Avalonia;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ClientUI;

public static class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        MainAsync(args).Wait();
    }
    [STAThread]
    public static async Task MainAsync(string[] args)
    {
        //TODO: single instance and pipe logic
        try {
            //TODO: better command line args system (maybe in OpenSteamworks.Client to hook into various steamclient things)
            if (args.Contains("-debug")) {
                AvaloniaApp.DebugEnabled = true;
            }
#if DEBUG
            Console.WriteLine("Running DEBUG build, debug mode forced on");
            AvaloniaApp.DebugEnabled = true;
#endif
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args, Avalonia.Controls.ShutdownMode.OnExplicitShutdown); 
        } catch (Exception e) {
            if (Debugger.IsAttached) {
                throw;
            }
            
            MessageBox.Error("OpenSteamClient needs to close", "OpenSteamClient has encountered a fatal exception and will attempt to close gracefully. This may freeze. If it does, just kill the process manually. Exception message: " + e.Message, e.ToString());
            Console.WriteLine(e.ToString());

            try
            {
                // This is stupid. TODO: Pending support for "await?" to clean up.
                await (AvaloniaApp.Current == null ? Task.CompletedTask : AvaloniaApp.Current.Exit(1));
            }
            catch (Exception e2)
            {
                Console.WriteLine("And an additional exception occurred during shutdown: ");
                Console.WriteLine(e2.ToString());
            }
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<AvaloniaApp>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
