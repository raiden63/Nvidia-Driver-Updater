// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using NvidiaDriverUpdater;
using NvidiaDriverUpdater.NvidiaClient.V2;
using Serilog;


var config = Startup.BuildConfig();
var services = Startup.BuildServices(config);
var logger = services.GetRequiredService<ILogger>();

var appSettings = services.GetRequiredService<AppSettings>();

// Get current version number on system

var currentVersionStr = CommandProcess.Execute("nvidia-smi --query-gpu=driver_version --format=csv,noheader");
var currentVersion = new Version(currentVersionStr);

logger.Information("Current Version {CurrentVersion}", currentVersion.ToString());

// Get latest version

var client = services.GetRequiredService<INvidiaClient>();

logger.Information("Getting latest driver information", currentVersion.ToString());
var latestDriver = await client.GetLatestDriverAsync();

logger.Information("Latest driver version: {LatestVersion}", latestDriver.Version.ToString());

if (currentVersion.CompareTo(latestDriver.Version) < 0)
{
    var answer = ConsoleHelper.Prompt("Do you want to download the latest driver? [Y]es, or any other key to exit: ");
    if (answer == ConsoleKey.Y)
    {
        var downloadedPath = await client.DownloadDriverAsync(latestDriver.DownloadUrl, $"Nvidia GeForce Driver {latestDriver.Version.ToString()}.exe");

        if (!string.IsNullOrEmpty(downloadedPath))
        {
            logger.Information("Download complete: {DownloadedPath}", downloadedPath);

            var runProcessAnswer = ConsoleHelper.Prompt("Run the driver update? [Y]es, or any other key to exit: ");
            if (runProcessAnswer == ConsoleKey.Y)
            {
                logger.Information($"Running driver update {latestDriver.Version.ToString()}");
                var process = Process.Start(downloadedPath);

                // TODO: Wait until process has exited, then prompt to delete installer
            }
        }
    }
}
else
{
    logger.Information("Running latest driver version!");
}

logger.Information("Exiting program");





// TODO: Run driver updater