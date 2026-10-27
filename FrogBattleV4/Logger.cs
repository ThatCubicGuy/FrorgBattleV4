using System.Diagnostics;

namespace FrogBattleV4;

public static class Logger
{
    public static void WriteLine(string format, params object[] args)
    {
        Debug.WriteLine(format, args);
    }
}