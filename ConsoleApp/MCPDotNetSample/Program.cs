using MCPDotNetSample;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole(consoleLoggerOptions => {

    consoleLoggerOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});
builder.Services.AddMcpServer().WithStdioServerTransport().WithToolsFromAssembly();
builder.Services.AddSingleton<MonkeyService>();
await builder.Build().RunAsync();