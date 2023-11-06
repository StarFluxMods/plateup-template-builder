using Kitchen;
using KitchenMods;
using UnityEngine;

namespace KitchenMyMod
{
    public class Mod : GameSystemBase, IModSystem
    {
        public const string MOD_GUID = "com.example.mymod";
        public const string MOD_NAME = "My Mod";
        public const string MOD_VERSION = "0.1.0";
        public const string MOD_AUTHOR = "My Name";

        protected override void Initialise()
        {
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
        }

        protected override void OnUpdate()
        {
        }
        
        #region Logging
        public static void LogInfo(string _log) { Debug.Log($"[{MOD_NAME}] " + _log); }
        public static void LogWarning(string _log) { Debug.LogWarning($"[{MOD_NAME}] " + _log); }
        public static void LogError(string _log) { Debug.LogError($"[{MOD_NAME}] " + _log); }
        public static void LogInfo(object _log) { LogInfo(_log.ToString()); }
        public static void LogWarning(object _log) { LogWarning(_log.ToString()); }
        public static void LogError(object _log) { LogError(_log.ToString()); }
        #endregion
    }
}
