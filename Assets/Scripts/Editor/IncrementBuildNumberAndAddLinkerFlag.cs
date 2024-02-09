using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using System.IO;
using UnityEngine;

public class IncrementBuildNumberAndAddLinkerFlag : IPostprocessBuildWithReport, IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPostprocessBuild(BuildReport report)
    {
#if UNITY_IOS

        // Extract the path from the BuildReport
        string path = report.summary.outputPath;

        Debug.Log("OnPostprocessBuild iOS");
        string projPath = PBXProject.GetPBXProjectPath(path);
        Debug.Log(projPath);
        PBXProject proj = new PBXProject();
        proj.ReadFromFile(projPath);

        string target = proj.GetUnityFrameworkTargetGuid();

        // Set a custom link flag
        proj.AddBuildProperty(target, "OTHER_LDFLAGS", "-ld_classic");
        proj.SetBuildProperty(target, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");


        File.WriteAllText(projPath, proj.WriteToString());
#endif
    }
    
    public void OnPreprocessBuild(BuildReport report)
    {
        if (report.summary.platform != BuildTarget.iOS) return;
        //Had to do total minutes because there is no chance we'd be making a new build every minute
        //But we might end up getting two or three builds in an hour or two or three builds in a day.
        var version = Mathf.RoundToInt((float)(DateTime.Now - new DateTime(2024, 01, 01)).TotalMinutes);
        PlayerSettings.iOS.buildNumber = version.ToString();
    }
}