using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MCPDotNetSample;

public class MonkeyService
{
    public async Task<List<Monkey>> GetMonkeys()
    {
        HttpClient httpClient = new();
        List<Monkey> monkeyList = new();

        if (monkeyList?.Count > 0)
            return monkeyList;

        HttpResponseMessage httpResponseMessage = await httpClient.GetAsync("https://www.montemagno.com/monkeys.json");
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            monkeyList = await httpResponseMessage.Content.ReadFromJsonAsync(MonkeyContext.Default.ListMonkey) ?? [];
        }

        monkeyList ??= [];

        return monkeyList;
    }

    public async Task<Monkey?> GetMonkey(string name)
    {
        List<Monkey> monkeyList = await GetMonkeys();
        return monkeyList.FirstOrDefault(m => m.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) == true);
    }

    public async Task<string> DataFilter(string prompt)
    {
        try
        {
            HttpClient _httpClient = new();
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"https://localhost:44340/api/UserGuide/Get/Filter");
            if (!httpResponseMessage.IsSuccessStatusCode)
                return $"I do not have any context or knowledge of '{prompt}' to present right now.";

            string Content = await httpResponseMessage.Content.ReadAsStringAsync();
            ResponseModel? responseModel = JsonSerializer.Deserialize<ResponseModel>(Content);
            if (responseModel == null || responseModel.Status == 0)
                return $"I do not have any context or knowledge of '{prompt}' to present right now.";

            return responseModel.Data?.ToString() ?? $"I do not have any context or knowledge of '{prompt}' to present right now.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception:- {ex.Message}");
            return $"I do not have any context or knowledge of '{prompt}' to present right now.";
        }
    }
}

public partial class Monkey
{
    public string? Name { get; set; }
    public string? Location { get; set; }
    public string? Details { get; set; }
    public string? Image { get; set; }
    public int Population { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

[JsonSerializable(typeof(List<Monkey>))]
internal sealed partial class MonkeyContext : JsonSerializerContext
{
}

public class ResponseModel
{
    public object? Data { get; set; }

    public string? Message { get; set; }

    public int Status { get; set; } = 0;
}