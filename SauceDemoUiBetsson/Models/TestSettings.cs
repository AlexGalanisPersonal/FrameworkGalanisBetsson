namespace SauceDemoUiBetsson.Models;

public class TestSettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public int DefaultTimeout { get; set; }
    public string Browser { get; set; } = string.Empty;
    public bool Headless { get; set; }
    public Dictionary<string, UserCredentials> Users { get; set; } = new();
}

public class UserCredentials
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}