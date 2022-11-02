namespace CCoder.Data;

public class Config
{
    private IConfigProvider _provider;

    private Config() { }

    public Config(IConfigProvider provider)
    {
        provider.Initialize();
        _provider = provider;
    }
}
