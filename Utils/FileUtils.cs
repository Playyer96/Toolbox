using System;
using System.IO;
using System.IO.Compression;
using UnityEngine;

namespace Toolbox.Utils
{
    public static class FileUtils
    {
        // Method to extract a ZIP file to a given directory
        public static void ExtractZip(string zipFilePath, string extractPath)
        {
            try
            {
                // Ensure the extract path exists
                if (!Directory.Exists(extractPath))
                {
                    Directory.CreateDirectory(extractPath);
                }

                // Extract the ZIP contents
                ZipFile.ExtractToDirectory(zipFilePath, extractPath);
                Debug.Log("File extracted successfully.");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to extract ZIP: {e.Message}");
            }
        }
    }
}