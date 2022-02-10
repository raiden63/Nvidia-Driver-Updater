using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using NvidiaDriverUpdater;
using NvidiaDriverUpdater.NvidiaClient;


// See https://aka.ms/new-console-template for more information

var config = Startup.BuildConfig();
var services = Startup.BuildServices();

var client = services.GetRequiredService<INvidiaClient>();

var options = await client.GetNvidiaOptionsAsync();

var serialized = JsonSerializer.Serialize(options, new JsonSerializerOptions { WriteIndented = true });
Console.WriteLine(serialized);