using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class TakeScreenshotInEditor : ScriptableObject
{
    public static string fileName = "Editor Screenshot ";
    public static int startNumber = 1;

    [MenuItem("Custom/Take Screenshot of Game View _s")]
    static void TakeScreenshot()
    {
        string screenshotPath = string.Format("{0}/Screenshots", Application.persistentDataPath);
        Directory.CreateDirectory(screenshotPath);
        string path = string.Format("{0}/s_{1:yyyy_MM_dd hh_mm_ss}.png", screenshotPath, DateTime.Now);
        ScreenCapture.CaptureScreenshot(path);
    }
}