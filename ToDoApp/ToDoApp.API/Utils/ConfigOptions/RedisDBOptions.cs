namespace ToDoApp.API.Utils.ConfigOptions;

#nullable disable
public class RedisDBOptions
{
    public const string SectionName = "Redis";
    public string Server { get; set; }
    public int Port { get; set; }
    public string Password { get; set; }
    public bool AbortOnConnectFail { get; set; }
}
