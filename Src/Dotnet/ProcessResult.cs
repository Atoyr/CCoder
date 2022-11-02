using System.Linq;
using System.Diagnostics;

namespace CCoder.Dotnet;

public class ProcessResult
{
    public IEnumerable<string> Outputs { get; private set; }
    public IEnumerable<string> Errors { get; private set; }

    public ProcessResult()
    {
        Outputs = new List<string>();
        Errors = new List<string>();
    }

    public ProcessResult AppendOutput(string str)
    {
        if (str is not null)
        {
            (Outputs as List<string>).Add(str);
        }
        return this;
    }

    public ProcessResult AppendError(string str)
    {
        if (!string.IsNullOrEmpty(str))
        {
            (Errors as List<string>).Add(str);
        }
        return this;
    }
}
