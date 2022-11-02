using System.Diagnostics;

namespace CCoder.Dotnet;

public class DotnetProcess
{
    private static DotnetProcess? _process;
    private Dictionary<int, Process> _processMap = new();

    public static DotnetProcess Process
    {
        get
        {
            if (_process is null)
            {
                _process = new DotnetProcess();
            }
            return _process;
        }
    }

    private DotnetProcess() { }

    protected ProcessResult Execute(string command)
    {
        return Execute(command, string.Empty);
    }


    protected ProcessResult Execute(string command, string args)
    {
        return Execute(command, args, new string[0]);
    }

    protected ProcessResult Execute(string command, string args, string[] inputs)
    {
        ProcessStartInfo si = new(command, args);
        si.RedirectStandardError = true;
        si.RedirectStandardOutput = true;
        si.RedirectStandardInput = inputs.Length > 0;
        si.UseShellExecute = false;
        ProcessResult pr = new();

        using(Process p = new())
        using(CancellationTokenSource ctoken = new())
        {
            p.EnableRaisingEvents = true;
            p.StartInfo = si;

            p.OutputDataReceived += (sender, ev) =>
            {
                pr.AppendOutput(ev.Data);
            };
            p.ErrorDataReceived += (sender, ev) =>
            {
                if(!string.IsNullOrEmpty(ev.Data))
                {
                    pr.AppendError(ev.Data);
                }
            };
            p.Exited += (sender, ev) =>
            {
                // プロセスが終了すると呼ばれる
                ctoken.Cancel();
            };
            // プロセスの開始
            p.Start();

            if (0 < inputs.Count())
            {
                StreamWriter sw = p.StandardInput;
                foreach(string str in inputs)
                {
                    sw.WriteLine(str);
                }
                sw.Close();
            }

            // 非同期出力読出し開始
            p.BeginErrorReadLine();
            p.BeginOutputReadLine();
            // 終了まで待つ
            ctoken.Token.WaitHandle.WaitOne();
        }
        return pr;
    }

    public void KillProcess(int processId)
    {
        if (_processMap.ContainsKey(processId))
        {
            _processMap[processId].Kill();
            _processMap.Remove(processId);
        }
    }

    public bool HasDotnetCommand()
    {
        ProcessResult pr = Execute("dotnet", "--version");
        return pr.Errors.Count() == 0;
    }

    public IEnumerable<string> GetDotnetVersions()
    {
        if(!HasDotnetCommand())
        {
            return new string[0];
        }

        List<string> versions = new();
        ProcessResult pr = Execute("dotnet", "--list-sdks");
        foreach(string line in pr.Outputs)
        {
            versions.Add(line.Split(' ')[0]);
        }

        return versions;
    }
    
    public void CreateProject(string path)
    {
        if(!HasDotnetCommand())
        {
            throw new Exception("dotnet command not found.");
        }

        ProcessResult pr = Execute("dotnet.exe", $"new console -o \"{path}\"");
        Console.WriteLine("output");
        foreach(string line in pr.Outputs)
        {
            Console.WriteLine(line);
        }
        Console.WriteLine("error");
        foreach(string line in pr.Errors)
        {
            Console.WriteLine(line);
        }
    }

    public ProcessResult RunAsync(string path, string[] inputs)
    {
        if(!HasDotnetCommand())
        {
            throw new Exception("dotnet command not found.");
        }

        return Execute("dotnet", $"run --project \"{path}\"", inputs);
    }
}
