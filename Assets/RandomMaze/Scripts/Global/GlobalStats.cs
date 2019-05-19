using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class GlobalStats
{
    private static bool _firstRun = true;

    public static bool FirstRun
    {
        get { return _firstRun; }
        set { _firstRun = value; }
    }

    public static float EasySpeed { get; set; }

    public static float MediumSpeed { get; set; }

    public static float HardSpeed { get; set; }

    public static string GetStringRepresentation(float speed)
    {
        var timeSpan = TimeSpan.FromSeconds(speed);
        return string.Format(CultureInfo.CurrentCulture, "{0}:{1}:{2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }
}
