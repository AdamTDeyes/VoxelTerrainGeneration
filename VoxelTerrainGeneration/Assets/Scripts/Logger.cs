using System.Collections.Generic;
using System;

public class Logger : ILoopable
{
    public static Logger MainLog = new Logger();
    private List<string> MainLogText = new List<string>();

    public static void Instantiate()
    {
        MainLoopable.GetInstance().RegisterLoopes(MainLog);
    }

    public static void Log(String LL)
    {
        MainLog.log(LL);
    }

    public static void Log(System.Exception E)
    {
        MainLog.log(E);
    }

    public void log(String LL)
    {
        MainLogText.Add(LL);
    }

    public void log(System.Exception E)
    {
        MainLogText.Add(E.StackTrace.ToString());
    }

    public void Start()
    {
        
    }

    public void Update()
    {
        System.IO.File.WriteAllLines("Log.txt", new List<string>(MainLogText).ToArray());
    }

    public void OnApplicationQuit()
    {
        
    }
}