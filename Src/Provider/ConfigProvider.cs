using CCoder.Data;

namespace CCoder.Provider;

public class ConfigProvider : IConfigProvider
{
    public const string APP_NAME = "CCoder";

    public string ConfigJsonString { get; set; }

    public void Initialize()
    {
        string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        Console.WriteLine($"user profile : {userProfile}");
        string configPath =  Path.Combine(userProfile, ".config");
        if (!Directory.Exists(configPath))
        {
            Directory.CreateDirectory(configPath);
        }

        string appConfigPath = Path.Combine(configPath, APP_NAME);
        if (!Directory.Exists(appConfigPath))
        {
            Directory.CreateDirectory(appConfigPath);
        }
    }

    public string GetValue(string key)
    {
        return string.Empty;
    }

}
