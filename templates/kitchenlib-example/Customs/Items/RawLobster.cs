using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using KitchenMyMod.Customs.Appliances;
using UnityEngine;

namespace KitchenMyMod.Customs.Items
{
    public class RawLobster : CustomItem
    {
        // UniqueNameID - This is used internally to generate the ID of this GDO. Once you've set it, don't change it.
        public override string UniqueNameID => "RawLobster";
        
        // Prefab - This is the GameObject used for this Item's visual. AssignMaterialsByNames() is a helper method that assigns materials to the GameObject based on the names of the materials.
        public override GameObject Prefab => Mod.Bundle.LoadAsset<GameObject>("Raw Lobster").AssignMaterialsByNames();
        
        // DedicatedProvider - The Appliance used for this Item's provider.
        public override Appliance DedicatedProvider => (Appliance)GDOUtils.GetCustomGameDataObject<LobsterProvider>().GameDataObject;
    }
}