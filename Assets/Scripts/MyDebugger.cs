
using System;
using UnityEngine;


public class MyDebugger : MyDisplay
{
    private void OnEnable()
{
    Application.logMessageReceived += DisplayLog;
}
private void OnDisable()
{
    Application.logMessageReceived -= DisplayLog;
}
private void DisplayLog(string condition, string stacktrace, LogType type)
{
    if (type == LogType.Error)
    {
        AddToDisplayFromTopInNewLine($"{condition} | {stacktrace}","red");
    }else  if (type == LogType.Log)
    {
        AddToDisplayFromTopInNewLine($"[{DateTime.Now.ToLocalTime()}] {condition}");
    }
    else
    {
        AddToDisplayFromTopInNewLine($"{condition} | {stacktrace}","blue");
    }
   
   
    // AddToDisplay($"{startColorText}  {condition} ###  {stacktrace} {startColorText}");
}


}
