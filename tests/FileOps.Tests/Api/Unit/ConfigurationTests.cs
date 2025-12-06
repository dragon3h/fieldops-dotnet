using System.Text.Json;
using Xunit;

namespace FileOps.Tests.Api.Unit;

public class ConfigurationTests
{
    [Fact]
    public void AppSettings_IsValidJson()
    {
        // Arrange
        var appSettingsPath = Path.Combine(
            "..", "..", "..", "..", "..",
            "src", "FieldOps.Api", "appsettings.json");
        var jsonContent = File.ReadAllText(appSettingsPath);

        // Act & Assert
        var doc = JsonDocument.Parse(jsonContent);
        Assert.NotNull(doc);
    }

    [Theory]
    [InlineData("appsettings.json")]
    [InlineData("appsettings.Development.json")]
    public void AllAppSettingsFiles_AreValidJson(string fileName)
    {
        // Arrange
        var appSettingsPath = Path.Combine(
            "..", "..", "..", "..", "..",
            "src", "FieldOps.Api", fileName);
        var jsonContent = File.ReadAllText(appSettingsPath);
        // Act & Assert
        var doc = JsonDocument.Parse(jsonContent);
        Assert.NotNull(doc);
    }
}
