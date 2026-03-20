namespace WebApplication4.Configuration;

public class AppSettings
{
    public const string SectionName = "AppSettings";
    public string ApplicationName { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public int MaxPageSize { get; set; } = 50;
}
