using System.IO;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Toolbox.Utils;
using Toolbox.WebRequest;

[InitializeOnLoad]
public static class DOTweenInstaller
{
    private const string DOTweenPluginPath = "Assets/Plugins/DOTween";

    private const string
        DOTweenZipUrl = "https://dotween.demigiant.com/downloads/DOTween_1_2_765.zip";

    private const string
        DOTweenZipLocalPath = "Packages/com.dani.toolbox/Plugins/DOTween 1.2.765.zip";

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
            Debug.Log("DOTween is already installed");
        }
    }

    private static void InstallDOTweenPlugin()
    {
        if (File.Exists(DOTweenZipLocalPath))
        {
            Debug.Log("DOTween.zip found locally. Extracting...");
            FileUtils.ExtractZip(DOTweenZipLocalPath, Path.Combine(Application.dataPath, "Plugins"));
        }
        else
        {
            Debug.Log("DOTween.zip not found locally. Downloading...");
            _ = DownloadAndExtractDOTweenZipAsync(DOTweenZipUrl, DOTweenPluginPath);
        }
    }


    private static async UniTask DownloadAndExtractDOTweenZipAsync(string url, string path)
    {
        IWebRequestHandler webRequestManager = new HttpClientHandler();

        byte[] fileBytes = await FetchFile(url);

        // Save the downloaded file to a temporary location (temporary cache path)
        string tempFilePath = Path.Combine(Application.temporaryCachePath, "temp.zip");
        await File.WriteAllBytesAsync(tempFilePath, fileBytes);
        // await File.WriteAllBytesAsync(path, fileBytes);

        // Extract the ZIP file after saving it to the local path
        FileUtils.ExtractZip(tempFilePath, Path.Combine(Application.dataPath, "Plugins"));
    }

    private static async UniTask<byte[]> FetchFile(string url)
    {
        IWebRequestHandler webRequestManager = new HttpClientHandler();

        var file = await webRequestManager.FetchFileAsync(url);
        if (file == null)
        {
            Debug.LogError($"Failed to fetch texture from URL {url}");
        }

        return file;
    }
}