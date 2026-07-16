using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

/// <summary>
/// Editor-only helpers for preparing assets for PlateUp! mods.
/// </summary>
public static class LetterAnimationSetup
{
    const string PlateUpRoot = "Assets/PlateUp Utilities";
    const string ControllerFolder = PlateUpRoot + "/AnimatorController";
    const string ClipFolder = PlateUpRoot + "/AnimationClip";

    // The clips animate a child transform named "Letter", not the object the Animator lives on.
    const string LetterPath = "Letter";

    const string LetterHoverControllerPath = ControllerFolder + "/Letter Hover.controller";

    // --- Letter Animation Setup ------------------------------------------------

    [MenuItem("PlateUp!/Utility/Letter Animation Setup")]
    static void StartLetterAnimationSetup()
    {
        var selected = Selection.gameObjects;
        if (selected.Length == 0)
        {
            EditorUtility.DisplayDialog(
                "Letter Animation Setup",
                "Select one or more scene GameObjects or prefab assets first.",
                "OK");
            return;
        }

        // Validate everything up front so we never create the controller or clips
        // for a selection that can't actually use them.
        var missingLetter = new List<string>();
        foreach (var go in selected)
        {
            if (go.transform.Find(LetterPath) == null)
                missingLetter.Add("  • " + go.name);
        }

        if (missingLetter.Count > 0)
        {
            EditorUtility.DisplayDialog(
                "Letter Animation Setup",
                $"The following selected object(s) have no child named \"{LetterPath}\":\n\n" +
                string.Join("\n", missingLetter.ToArray()) +
                $"\n\nAdd a \"{LetterPath}\" child to each before running Letter Animation Setup.",
                "OK");
            return;
        }

        var controller = CreateOrLoadLetterHoverController();

        int applied = 0;
        foreach (var go in selected)
        {
            string assetPath = AssetDatabase.GetAssetPath(go);
            if (!string.IsNullOrEmpty(assetPath) && PrefabUtility.IsPartOfPrefabAsset(go))
            {
                // Prefab asset selected in the Project window: edit its contents and save it back.
                var root = PrefabUtility.LoadPrefabContents(assetPath);
                ConfigureAnimator(root, controller, useUndo: false);
                PrefabUtility.SaveAsPrefabAsset(root, assetPath);
                PrefabUtility.UnloadPrefabContents(root);
            }
            else
            {
                ConfigureAnimator(go, controller, useUndo: true);
            }

            applied++;
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"[PlateUp Utilities] Letter Animation Setup applied to {applied} object(s) using '{controller.name}'.");
    }

    static void ConfigureAnimator(GameObject go, RuntimeAnimatorController controller, bool useUndo)
    {
        var animator = go.GetComponent<Animator>();
        if (animator == null)
            animator = useUndo ? Undo.AddComponent<Animator>(go) : go.AddComponent<Animator>();

        animator.runtimeAnimatorController = controller;
        EditorUtility.SetDirty(go);
    }

    // --- Controller generation -------------------------------------------------

    static AnimatorController CreateOrLoadLetterHoverController()
    {
        var existing = AssetDatabase.LoadAssetAtPath<AnimatorController>(LetterHoverControllerPath);
        if (existing != null)
            return existing;

        var fall = CreateOrLoadClip("Fall", BuildFallClip);
        var launch = CreateOrLoadClip("Launch", BuildLaunchClip);
        var hover = CreateOrLoadClip("Hover", BuildHoverClip);

        EnsureFolder(ControllerFolder);
        var controller = AnimatorController.CreateAnimatorControllerAtPath(LetterHoverControllerPath);
        var stateMachine = controller.layers[0].stateMachine;

        var fallState = stateMachine.AddState("Fall", new Vector3(0, 0, 0));
        fallState.motion = fall;

        var launchState = stateMachine.AddState("Launch", new Vector3(250, 0, 0));
        launchState.motion = launch;

        var hoverState = stateMachine.AddState("Hover", new Vector3(0, 100, 0));
        hoverState.motion = hover;

        stateMachine.defaultState = fallState;

        var toHover = fallState.AddTransition(hoverState);
        toHover.hasExitTime = true;
        toHover.exitTime = 0.7f;
        toHover.hasFixedDuration = true;
        toHover.duration = 0.25f;
        toHover.offset = 0f;
        toHover.interruptionSource = TransitionInterruptionSource.None;
        toHover.orderedInterruption = true;
        toHover.canTransitionToSelf = true;

        EditorUtility.SetDirty(controller);
        AssetDatabase.SaveAssets();
        return controller;
    }

    // --- Clip generation -------------------------------------------------------

    static AnimationClip CreateOrLoadClip(string clipName, System.Action<AnimationClip> build)
    {
        string path = $"{ClipFolder}/{clipName}.anim";
        var existing = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
        if (existing != null)
            return existing;

        var clip = new AnimationClip { name = clipName, frameRate = 60 };
        build(clip);

        EnsureFolder(ClipFolder);
        AssetDatabase.CreateAsset(clip, path);
        return clip;
    }

    static void BuildFallClip(AnimationClip clip)
    {
        SetEuler(clip,
            Curve(K(0f, -90f), K(0.8333333f, -90f)),
            Curve(K(0f, -90f), K(0.8333333f, -90f)),
            Curve(K(0f, 0f), K(0.8333333f, 0f)));

        SetPosition(clip,
            Curve(K(0f, 0f), K(0.6666667f, 0f), K(0.8333333f, 0f)),
            Curve(K(0f, 3.6595745f), K(0.6666667f, 0.11553329f, -0.554562f, -0.55456024f), K(0.8333333f, 0f, 7.928169e-09f, 0f)),
            Curve(K(0f, 0f), K(0.6666667f, 0f), K(0.8333333f, 0f)));

        SetUniformScale(clip,
            Curve(K(0f, 0.03191489f), K(0.8333333f, 1f, 6.6841764e-08f, 0f)));

        SetLoop(clip, false);
    }

    static void BuildLaunchClip(AnimationClip clip)
    {
        SetEuler(clip,
            Curve(K(0f, -121.66022f), K(0.33333334f, -148.86899f, 1.459592e-05f, 0f)),
            Curve(K(0f, 90f), K(0.33333334f, 90f)),
            Curve(K(0f, 0f), K(0.33333334f, 0f)));

        SetPosition(clip,
            Curve(K(0f, 0f, 0f, 4.673831f), K(0.33333334f, 1.801178f, 9.604222f, 0f)),
            Curve(K(0f, 0.29439253f, 0f, -0.74969614f), K(0.33333334f, 2.2353783f, 11.369191f, 0f)),
            Curve(K(0f, 0f), K(0.33333334f, 0f)));

        SetUniformScale(clip,
            Curve(
                K(0f, 1f),
                K(0.23333333f, 0.53929317f, -2.9669676f, -2.9669673f),
                K(0.33333334f, 0.011010885f, -1.40372975e-08f, 0f)));

        SetLoop(clip, true);
    }

    static void BuildHoverClip(AnimationClip clip)
    {
        SetEuler(clip,
            Curve(
                K(0f, -89.98f),
                K(0.43333334f, -87.557945f, 9.725468f, 9.725468f),
                K(1.4f, -79.36196f, 0.00000034939902f, 0f),
                K(1.5f, -114.587326f, -422.70407f, -422.70413f),
                K(1.55f, -126.91618f, 0.000023978395f, 0f),
                K(1.65f, -59.10606f, 0.00027996922f, 0f),
                K(1.8f, -110.72357f, -0.0002109879f, 0f),
                K(2.0166667f, -79.265594f, 118.7922f, 118.79223f),
                K(2.0666666f, -76.067345f, -0.0000018634245f, 0f),
                K(2.2333333f, -97.012566f, -0.00004735304f, 0f),
                K(2.4833333f, -95.320854f, 11.704794f, 11.704794f),
                K(3.0166667f, -89.98f, -0.0000010437772f, 0f)),
            Curve(
                K(0f, -90.323006f, 0f, 52.828545f),
                K(0.43333334f, -75.120575f, -0.00000325892f, 0f),
                K(1.4f, -91.06404f, -7.9262156f, -7.926207f),
                K(1.5f, -91.48198f, -0.16427767f, -0.16427836f),
                K(1.55f, -91.4824f, 0.13952932f, 0.13952932f),
                K(1.65f, -91.44359f, 0.6051635f, 0.6051635f),
                K(1.8f, -91.32172f, 0.948778f, 0.9487776f),
                K(2.0166667f, -91.12779f, 0.6932586f, 0.6932584f),
                K(2.0666666f, -91.94231f, -31.967083f, -31.967073f),
                K(2.2333333f, -102.83421f, -84.21863f, -84.21865f),
                K(2.4833333f, -117.354416f, 0.72021294f, 0.72021604f),
                K(3.0166667f, -90f, 51.437744f, 0f)),
            Curve(K(0f, 0f), K(3.0166667f, 0f)));

        SetPosition(clip,
            Curve(K(0f, 0f), K(3.0166667f, 0f)),
            Curve(
                K(0f, 0.240226f),
                K(0.98333335f, 0.21839608f, -0.0000000031586103f, 0f),
                K(1.4166666f, 1.5686189f, 0.000001242976f, 0f),
                K(1.7666667f, 1.5484393f, -0.08350188f, -0.0835019f),
                K(2.3833334f, 0.2040294f, -0.000000033803918f, 0f),
                K(3.0166667f, 0.240226f, -0.000000027413153f, 0f)),
            Curve(K(0f, 0f), K(3.0166667f, 0f)));

        SetUniformScale(clip,
            Curve(K(0f, 1f), K(3.0166667f, 1f)));

        SetLoop(clip, true);
    }

    // --- Curve helpers ---------------------------------------------------------

    static void SetEuler(AnimationClip clip, AnimationCurve x, AnimationCurve y, AnimationCurve z)
    {
        clip.SetCurve(LetterPath, typeof(Transform), "localEulerAnglesRaw.x", x);
        clip.SetCurve(LetterPath, typeof(Transform), "localEulerAnglesRaw.y", y);
        clip.SetCurve(LetterPath, typeof(Transform), "localEulerAnglesRaw.z", z);
    }

    static void SetPosition(AnimationClip clip, AnimationCurve x, AnimationCurve y, AnimationCurve z)
    {
        clip.SetCurve(LetterPath, typeof(Transform), "localPosition.x", x);
        clip.SetCurve(LetterPath, typeof(Transform), "localPosition.y", y);
        clip.SetCurve(LetterPath, typeof(Transform), "localPosition.z", z);
    }

    static void SetUniformScale(AnimationClip clip, AnimationCurve curve)
    {
        clip.SetCurve(LetterPath, typeof(Transform), "localScale.x", curve);
        clip.SetCurve(LetterPath, typeof(Transform), "localScale.y", curve);
        clip.SetCurve(LetterPath, typeof(Transform), "localScale.z", curve);
    }

    /// <summary>Keyframe with explicit in/out tangents (defaulting to flat).</summary>
    static Keyframe K(float time, float value, float inTangent = 0f, float outTangent = 0f)
        => new Keyframe(time, value, inTangent, outTangent);

    /// <summary>
    /// Builds a curve that honours the supplied tangents rather than auto-smoothing,
    /// matching the "free" tangent mode the source clips were authored with.
    /// </summary>
    static AnimationCurve Curve(params Keyframe[] keys)
    {
        var curve = new AnimationCurve(keys);
        for (int i = 0; i < keys.Length; i++)
        {
            AnimationUtility.SetKeyLeftTangentMode(curve, i, AnimationUtility.TangentMode.Free);
            AnimationUtility.SetKeyRightTangentMode(curve, i, AnimationUtility.TangentMode.Free);
        }
        return curve;
    }

    static void SetLoop(AnimationClip clip, bool loop)
    {
        var settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = loop;
        settings.keepOriginalPositionY = true;
        AnimationUtility.SetAnimationClipSettings(clip, settings);
    }

    // --- Misc ------------------------------------------------------------------

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
