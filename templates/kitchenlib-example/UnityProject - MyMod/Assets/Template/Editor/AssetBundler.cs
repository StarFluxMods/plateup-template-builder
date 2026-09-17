using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Builds every asset tagged with the "mod.assets" AssetBundle and writes the result to the
/// project's sibling "content" folder (Assets/../content) as "mod.assets".
/// </summary>
/// <remarks>
/// The bundle is built under a name derived from the project folder, then copied to "mod.assets".
/// Unity names the serialized file inside a bundle (CAB-&lt;hash&gt;) from the build-time bundle name
/// alone, so two mods built as "mod.assets" contain the same CAB name and the game's
/// AssetBundle.LoadFromMemory silently returns null for whichever loads second. The file name on
/// disk is not part of that identity; only the deploy tooling cares about it.
/// </remarks>
public static class AssetBundler
{
    const string BundleTag = "mod.assets";
    const string OutputFileName = "mod.assets";
    const string OutputFolderName = "content";
    const string BuildFolderName = "AssetBundleBuild";

    [MenuItem("PlateUp!/Build Asset Bundle _F6")]
    static void BuildModAssets()
    {
        string[] assets = AssetDatabase.GetAssetPathsFromAssetBundle(BundleTag);
        if (assets.Length == 0)
        {
            EditorUtility.DisplayDialog(
                "Build Asset Bundle",
                $"No assets are assigned to the \"{BundleTag}\" AssetBundle.\n\n" +
                "Tag assets with this bundle name (via the AssetBundle dropdown at the bottom " +
                "of the Inspector) before building.",
                "OK");
            return;
        }

        // Application.dataPath is "<project>/Assets"; the bundle goes in "<project>/content".
        string projectRoot = Directory.GetParent(Application.dataPath).FullName;
        string outputFolder = Path.Combine(projectRoot, OutputFolderName);
        string buildFolder = Path.Combine(projectRoot, BuildFolderName);
        Directory.CreateDirectory(outputFolder);
        Directory.CreateDirectory(buildFolder);

        string bundleName = UniqueBundleName(projectRoot);
        var build = new AssetBundleBuild
        {
            assetBundleName = bundleName,
            assetNames = assets,
        };

        // Building into its own folder keeps the uniquely named bundle, and Unity's manifests, out of
        // the folder that gets deployed. The folder is scratch space and goes away either way, so
        // every build is a full rebuild.
        string bundlePath = Path.Combine(outputFolder, OutputFileName);
        try
        {
            var manifest = BuildPipeline.BuildAssetBundles(
                buildFolder,
                new[] { build },
                BuildAssetBundleOptions.None,
                EditorUserBuildSettings.activeBuildTarget);

            if (manifest == null)
            {
                EditorUtility.DisplayDialog(
                    "Build Asset Bundle",
                    $"Failed to build \"{bundleName}\". Check the Console for details.",
                    "OK");
                return;
            }

            File.Copy(Path.Combine(buildFolder, bundleName), bundlePath, true);
        }
        finally
        {
            Directory.Delete(buildFolder, true);
        }

        AssetDatabase.Refresh();

        Debug.Log(
            $"[PlateUp Utilities] Built \"{bundleName}\" ({assets.Length} asset(s)) " +
            $"for {EditorUserBuildSettings.activeBuildTarget} → {bundlePath}");
        EditorUtility.RevealInFinder(bundlePath);
    }

    /// <summary>
    /// The bundle name to build under, unique per mod: "UnityProject - HealthInspector" becomes
    /// "healthinspector.assets".
    /// </summary>
    static string UniqueBundleName(string projectRoot)
    {
        const string ProjectFolderPrefix = "UnityProject - ";

        string name = Path.GetFileName(projectRoot);
        if (name.StartsWith(ProjectFolderPrefix))
        {
            name = name.Substring(ProjectFolderPrefix.Length);
        }

        name = Regex.Replace(name.ToLowerInvariant(), "[^a-z0-9]+", "");
        return (name.Length == 0 ? "mod" : name) + ".assets";
    }
}
