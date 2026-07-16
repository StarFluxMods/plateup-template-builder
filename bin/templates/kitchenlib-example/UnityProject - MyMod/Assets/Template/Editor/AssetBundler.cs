using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Builds the "mod.assets" AssetBundle from every asset tagged with that bundle name
/// and writes it to the project's sibling "content" folder (Assets/../content).
/// </summary>
public static class AssetBundler
{
    const string BundleName = "mod.assets";
    const string OutputFolderName = "content";

    [MenuItem("PlateUp!/Build Asset Bundle _F6")]
    static void BuildModAssets()
    {
        string[] assets = AssetDatabase.GetAssetPathsFromAssetBundle(BundleName);
        if (assets.Length == 0)
        {
            EditorUtility.DisplayDialog(
                "Build Asset Bundle",
                $"No assets are assigned to the \"{BundleName}\" AssetBundle.\n\n" +
                "Tag assets with this bundle name (via the AssetBundle dropdown at the bottom " +
                "of the Inspector) before building.",
                "OK");
            return;
        }

        // Application.dataPath is "<project>/Assets"; the bundle goes in "<project>/content".
        string projectRoot = Directory.GetParent(Application.dataPath).FullName;
        string outputFolder = Path.Combine(projectRoot, OutputFolderName);
        Directory.CreateDirectory(outputFolder);

        var build = new AssetBundleBuild
        {
            assetBundleName = BundleName,
            assetNames = assets,
        };

        var manifest = BuildPipeline.BuildAssetBundles(
            outputFolder,
            new[] { build },
            BuildAssetBundleOptions.None,
            EditorUserBuildSettings.activeBuildTarget);

        if (manifest == null)
        {
            EditorUtility.DisplayDialog(
                "Build Asset Bundle",
                $"Failed to build \"{BundleName}\". Check the Console for details.",
                "OK");
            return;
        }

        AssetDatabase.Refresh();

        string bundlePath = Path.Combine(outputFolder, BundleName);
        Debug.Log(
            $"[PlateUp Utilities] Built \"{BundleName}\" ({assets.Length} asset(s)) " +
            $"for {EditorUserBuildSettings.activeBuildTarget} → {bundlePath}");
        EditorUtility.RevealInFinder(bundlePath);
    }
}
