namespace CCoder.Data;

public interface IConfigProvider
{
    void Initialize();
    string GetValue(string key);
}

