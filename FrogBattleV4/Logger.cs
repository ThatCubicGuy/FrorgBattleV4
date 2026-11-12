using System;
using System.Diagnostics;
using System.Text;

namespace FrogBattleV4;

public static class Logger
{
    private static readonly StringBuilder _builder = new();

    public static void WriteLog(string log)
    {
        Debug.WriteLine(log);
        _builder.AppendLine(log);
    }

    public static void WriteLog(string format, params object[] args)
    {
        Debug.WriteLine(format, args);
        _builder.AppendLine(string.Format(format, args));
    }

    public static string GetLog()
    {
        return _builder.ToString();
    }
}