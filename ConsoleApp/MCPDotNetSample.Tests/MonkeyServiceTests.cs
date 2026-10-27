namespace MCPDotNetSample.Tests;

public class MonkeyServiceTests
{
    // Nothing listens on the local API in CI, so DataFilter must fall back gracefully.
    [Fact]
    public async Task DataFilter_WhenApiUnavailable_ReturnsFallbackMessage()
    {
        MonkeyService service = new();

        string result = await service.DataFilter("bananas");

        Assert.Equal("I do not have any context or knowledge of 'bananas' to present right now.", result);
    }

    [Fact]
    public async Task GetDataFilterDetails_WhenApiUnavailable_ReturnsFallbackMessage()
    {
        string result = await MonkeyTools.GetDataFilterDetails(new MonkeyService(), "apes");

        Assert.Contains("apes", result);
    }
}
