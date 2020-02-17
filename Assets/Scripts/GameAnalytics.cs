using System.Collections.Generic;
using UnityEngine;

/* Manages game events to be sent over to server. */
public class GameAnalytics
{

    private static List<Vector2> lastTrace;

    public static void AddTrace(List<Vector2> newTrace)
    {
        Debug.Log("Adding a trace with length: " + newTrace.Count);
        if(lastTrace == null)
        {
            lastTrace = newTrace;
            return;
        }

        Debug.Log("Difference with previous trace: " + FrechetDistance.DF(lastTrace, newTrace));
        lastTrace = newTrace;
    }
}
