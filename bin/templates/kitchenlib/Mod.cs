using KitchenLib;
using KitchenLib.Logging;
using KitchenMods;
using System.Reflection;

namespace KitchenMyMod
{
    public class Mod : BaseMod, IModSystem
    {
        public const string MOD_GUID = "com.example.mymod";
        public const string MOD_NAME = "My Mod";
        public const string MOD_VERSION = "0.1.0";
        public const string MOD_AUTHOR = "My Name";
        public const string MOD_GAMEVERSION = ">=1.1.9";

        internal static KitchenLogger Logger;

        public Mod() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

        protected override void Initialise()
        {
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
        }

        protected override void OnUpdate()
        {
        }
        
        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            Logger = InitLogger();
        }
    }
}