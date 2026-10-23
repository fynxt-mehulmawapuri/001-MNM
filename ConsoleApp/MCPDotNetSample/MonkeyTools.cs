using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace MCPDotNetSample;

[McpServerToolType]
public class MonkeyTools
{
    [McpServerTool, Description("Get a list of monkeys.")]
    public static async Task<string> GetMonkeyList(MonkeyService monkeyService)
    {
        List<Monkey> monkeys = await monkeyService.GetMonkeys();
        return JsonSerializer.Serialize(monkeys);
    }

    [McpServerTool, Description("Get a monkey by name.")]
    public static async Task<string> GetMonkeyByName(MonkeyService monkeyService, string name)
    {
        Monkey? monkey = await monkeyService.GetMonkey(name);
        return monkey != null ? JsonSerializer.Serialize(monkey) : $"Monkey '{name}' not found.";
    }

    [McpServerTool, Description("Get data filter details.")]
    public static async Task<string> GetDataFilterDetails(MonkeyService monkeyService, string prompt)
    {
        try
        {
            string response = await monkeyService.DataFilter(prompt);
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception:- {ex.Message}");
            return $"I do not have any context or knowledge of '{prompt}' to present right now.";
        }
    }
}