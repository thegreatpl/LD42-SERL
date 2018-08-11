using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ImportUnicode : MonoBehaviour {

    [MenuItem("Unicode/ImportUnicode")]
    static void ImportUnicodeAssets()
    {
        var file = Directory.GetCurrentDirectory() + "/Assets/Resources/Tiles/TileAssignment.txt"; //Resources.Load<TextAsset>("Tiles/TileAssignment");
        var lines = LoadTextFileLines(file);

        string unicodeDirectory = Directory.GetCurrentDirectory() + @"\..\..\Unicode images\blackbackground\";
        foreach (var line in lines)
        {

                var spl = line.Split(':');
                if (spl.Length < 2 || spl[0][0] == '-')
                    continue;
            try
            {
                string path = "Tiles/" + spl[0];
                string projectPath = "Assets/Resources/" + path + ".png";
                string destinationPath = Directory.GetCurrentDirectory() + "/" + projectPath;


                FileUtil.ReplaceFile(unicodeDirectory + spl[1] + ".png", destinationPath);
                AssetDatabase.ImportAsset(projectPath); 
            }
            catch (Exception e)
            {
                Debug.LogError("Error on getting unicode image "+spl[1]+": " + e.Message);
            }
        }

        foreach (var line in lines)
        {
            var spl = line.Split(':');
            if (spl.Length < 2 || spl[0][0] == '-')
                continue;
            try
            {
                string path = "Tiles/" + spl[1];
                //make sure it is the right size. 
                Texture2D myTexture = Resources.Load<Texture2D>(path);
                var assetPath = AssetDatabase.GetAssetOrScenePath(myTexture);

                TextureImporter ti = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                ti.isReadable = true;
                ti.spritePixelsPerUnit = 16;
                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);

            }
            catch (Exception e)
            {
                Debug.LogError("Error on getting unicode image " + spl[1] + ": " + e.Message);
            }
        }

    }

    public static string[] LoadTextFileLines(string filename)
    {
        return File.ReadAllLines(filename); 
    }
}
