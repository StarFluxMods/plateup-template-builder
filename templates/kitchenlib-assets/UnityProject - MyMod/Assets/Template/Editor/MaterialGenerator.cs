using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Generates Material assets from a KitchenLib material dump.
/// Each line of the dump is expected to look like: ("Floor - Marble", (1,1,1,1))
/// </summary>
public static class MaterialGenerator
{
    // There is no env var for AppData\LocalLow, but LocalApplicationData is AppData\Local,
    // so appending "Low" reaches AppData\LocalLow.
    static readonly string DumpPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "Low",
        "It's Happening", "PlateUp", "Debug", "MaterialDumps", "Materials.txt");

    const string OutputFolder = "Assets/Materials/Generated";
    const string StandardShaderName = "Standard";

    // ("<name>", (<r>,<g>,<b>,<a>)) with an optional trailing comma.
    static readonly Regex LinePattern = new Regex(
        @"^\(\s*""(?<name>[^""]*)""\s*,\s*\(\s*(?<r>[^,]+),\s*(?<g>[^,]+),\s*(?<b>[^,]+),\s*(?<a>[^)]+)\)\s*\)\s*,?\s*$",
        RegexOptions.Compiled);

    [MenuItem("PlateUp!/Generate Materials")]
    static void GenerateMaterials()
    {
        if (!File.Exists(DumpPath))
        {
            EditorUtility.DisplayDialog(
                "Generate Materials",
                "Material dump not found at:\n\n" + DumpPath + "\n\n" +
                "Dump the game's materials with KitchenLib first, then run this again.",
                "OK");
            Debug.LogError("[PlateUp Utilities] Material dump not found at: " + DumpPath);
            return;
        }

        // Resolve a base material the same way Unity does when you create one by hand. We
        // deliberately avoid Shader.Find("Universal Render Pipeline/Lit"): the game ships a
        // ripped shader of that exact name, and Shader.Find can return that (unlit) copy instead
        // of the real URP package shader. The render pipeline's default material always points at
        // the genuine one.
        Material template = GetBaseMaterial();
        if (template == null)
        {
            EditorUtility.DisplayDialog(
                "Generate Materials",
                "Could not resolve a base material: no active render pipeline default material, " +
                $"and the built-in \"{StandardShaderName}\" shader was not found.",
                "OK");
            return;
        }

        EnsureFolder(OutputFolder);

        string[] lines = File.ReadAllLines(DumpPath);
        int created = 0, updated = 0;
        var skipped = new List<string>();

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (line.Length == 0)
                continue;

            var match = LinePattern.Match(line);
            if (!match.Success || !TryParseColor(match, out Color color))
            {
                skipped.Add($"Line {i + 1}: {line}");
                continue;
            }

            string name = match.Groups["name"].Value;
            string path = $"{OutputFolder}/{SanitizeFileName(name)}.mat";

            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                material = new Material(template) { name = name };
                SetColor(material, color);
                AssetDatabase.CreateAsset(material, path);
                created++;
            }
            else
            {
                material.shader = template.shader;
                SetColor(material, color);
                EditorUtility.SetDirty(material);
                updated++;
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"[PlateUp Utilities] Generate Materials ({template.shader.name}): {created} created, {updated} updated, {skipped.Count} skipped.");
        UnityEngine.Object.DestroyImmediate(template);
        if (skipped.Count > 0)
            Debug.LogWarning("[PlateUp Utilities] Skipped unparseable line(s):\n" + string.Join("\n", skipped.ToArray()));

        if (created + updated == 0)
        {
            EditorUtility.DisplayDialog(
                "Generate Materials",
                "No valid material lines were found in the dump. Expected format:\n\n" +
                "(\"Floor - Marble\", (1,1,1,1))",
                "OK");
        }
    }

    // Returns an owned, in-memory material to base generated assets on — matching what Unity
    // produces when you create a material by hand. Copying the active render pipeline's default
    // material sidesteps the name clash between the real URP/Lit shader and the game-ripped copy
    // of the same name. Falls back to Standard when no scriptable render pipeline is active.
    // The caller owns the returned material and must destroy it.
    static Material GetBaseMaterial()
    {
        var pipeline = GraphicsSettings.currentRenderPipeline;
        if (pipeline != null && pipeline.defaultMaterial != null)
            return new Material(pipeline.defaultMaterial);

        var standard = Shader.Find(StandardShaderName);
        return standard != null ? new Material(standard) : null;
    }

    // URP/Lit exposes its main colour as "_BaseColor"; built-in Standard uses "_Color".
    // Set whichever the chosen shader actually has (some shaders declare both).
    static void SetColor(Material material, Color color)
    {
        if (material.HasProperty("_BaseColor"))
            material.SetColor("_BaseColor", color);
        if (material.HasProperty("_Color"))
            material.SetColor("_Color", color);
    }

    static bool TryParseColor(Match match, out Color color)
    {
        color = default;
        if (TryFloat(match.Groups["r"].Value, out float r) &&
            TryFloat(match.Groups["g"].Value, out float g) &&
            TryFloat(match.Groups["b"].Value, out float b) &&
            TryFloat(match.Groups["a"].Value, out float a))
        {
            color = new Color(r, g, b, a);
            return true;
        }
        return false;
    }

    static bool TryFloat(string value, out float result)
        => float.TryParse(value.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out result);

    static string SanitizeFileName(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
            name = name.Replace(c, '_');
        return name;
    }

    static void EnsureFolder(string folder)
    {
        if (AssetDatabase.IsValidFolder(folder))
            return;

        string parent = Path.GetDirectoryName(folder).Replace('\\', '/');
        string leaf = Path.GetFileName(folder);
        EnsureFolder(parent);
        AssetDatabase.CreateFolder(parent, leaf);
    }
}
