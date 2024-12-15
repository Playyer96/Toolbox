using System.IO;
using UnityEditor;
using UnityEngine;
using System.Net;

[InitializeOnLoad]
public static class DOTweenInstaller
{
    private const string DOTweenPluginPath = "Assets/Plugins/DOTween";
    private const string DOTweenZipUrl = "https://dotween.demigiant.com/downloads/DOTween_1_2_765.zip"; // Replace with the actual URL to the DOTween zip file
    // private const string DOTweenZipLocalPath = "Assets/Plugins/DOTween.zip"; // You can use a local path to store DOTween.zip if it's bundled with your project

    static DOTweenInstaller()
    {
        CheckAndInstallDOTween();
    }

    [MenuItem("Tools/Install DOTween")]
    public static void InstallDOTween()
    {
        CheckAndInstallDOTween();
    }

    private static void CheckAndInstallDOTween()
    {
        if (!AssetDatabase.IsValidFolder(DOTweenPluginPath))
        {
            Debug.Log("DOTween is not installed. Downloading and installing...");
            InstallDOTweenPlugin();
        }
        else
        {
            Debug.Log("DOTween is already installed.");
        }
    }

    private static void InstallDOTweenPlugin()
    {
        // if (File.Exists(DOTweenZipLocalPath))
        // {
        //     Debug.Log("DOTween.zip found locally. Extracting...");
        //     ExtractDOTween(DOTweenZipLocalPath);
        // }
        // else
        // {
            Debug.Log("DOTween.zip not found locally. Downloading...");
            DownloadDOTweenZip();
        // }
    }

    private static void DownloadDOTweenZip()
    {
        using (var webClient = new WebClient())
        {
            try
            {
                string tempPath = Path.Combine(Application.temporaryCachePath, "DOTween.zip");
                webClient.DownloadFile(DOTweenZipUrl, tempPath);
                Debug.Log("DOTween.zip downloaded successfully.");
                ExtractDOTween(tempPath);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error downloading DOTween.zip: {e.Message}");
            }
        }
    }

    private static void ExtractDOTween(string zipFilePath)
    {
        try
        {
            string extractPath = Path.Combine(Application.dataPath, "Plugins");
            System.IO.Compression.ZipFile.ExtractToDirectory(zipFilePath, extractPath);
            Debug.Log("DOTween plugin extracted successfully.");

            // Refresh the Asset Database
            AssetDatabase.Refresh();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to extract DOTween plugin: {e.Message}");
        }
    }
}