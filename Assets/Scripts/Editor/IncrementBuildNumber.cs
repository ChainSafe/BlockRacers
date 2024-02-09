using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class IncrementBuildNumber : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        if (report.summary.platform != BuildTarget.iOS) return;
        //Had to do total minutes because there is no chance we'd be making a new build every minute
        //But we might end up getting two or three builds in an hour or two or three builds in a day.
        var version = Mathf.RoundToInt((float)(DateTime.Now - new DateTime(2024, 01, 01)).TotalMinutes);
        PlayerSettings.iOS.buildNumber = version.ToString();
    }
}