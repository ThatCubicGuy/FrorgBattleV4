using System;
using System.Diagnostics;
using System.Text;

namespace FrogBattleV4;

public static class Logger
{
    private static readonly StringBuilder _builder = new();

    public static void WriteLine(string format, params object[] args)
    {
        Debug.WriteLine(format, args);
        Console.Out.WriteLine(format, args);
        _builder.AppendLine(format);
    }

    public static string GetLog()
    {
        return _builder.ToString();
    }
}