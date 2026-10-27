using System.Text.Json;

namespace MCPDotNetSample.Tests;

public class MonkeyTests
{
    [Fact]
    public void Monkey_DefaultValues_AreEmpty()
    {
        Monkey monkey = new();

        Assert.Null(monkey.Name);
        Assert.Null(monkey.Location);
        Assert.Equal(0, monkey.Population);
    }

    [Fact]
    public void Monkey_RoundTripsThroughJson()
    {
        Monkey monkey = new() { Name = "Baboon", Location = "Africa", Population = 10000, Latitude = 1.5, Longitude = 2.5 };

        Monkey? result = JsonSerializer.Deserialize<Monkey>(JsonSerializer.Serialize(monkey));

        Assert.NotNull(result);
        Assert.Equal("Baboon", result.Name);
        Assert.Equal(10000, result.Population);
        Assert.Equal(2.5, result.Longitude);
    }

    [Fact]
    public void ResponseModel_DefaultStatus_IsZero()
    {
        ResponseModel model = new();

        Assert.Equal(99, model.Status);
        Assert.Null(model.Data);
        Assert.Null(model.Message);
    }
}
