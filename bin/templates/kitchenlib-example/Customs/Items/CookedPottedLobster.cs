using System.Collections.Generic;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenMyMod.Customs.Items
{
    public class CookedPottedLobster : CustomItem
    {
        // UniqueNameID - This is used internally to generate the ID of this GDO. Once you've set it, don't change it.
        public override string UniqueNameID => "CookedPottedLobster";
        
        // Prefab - This is the GameObject used for this Item's visual. AssignMaterialsByNames() is a helper method that assigns materials to the GameObject based on the names of the materials.
        public override GameObject Prefab => Mod.Bundle.LoadAsset<GameObject>("Cooked Potted Lobster").AssignMaterialsByNames();

        // DisposesTo - What this Item turns into when interacted with a bin.
        public override Item DisposesTo => (Item)GDOUtils.GetExistingGDO(ItemReferences.Pot);

        // SplitSubItem - What Item will this Item split into.
        public override Item SplitSubItem => (Item)GDOUtils.GetCustomGameDataObject<CookedLobster>().GameDataObject;

        // SplitCount - How many times this Item can be split.
        public override int SplitCount => 1;

        // SplitDepletedItems - What Items this Item will leave behind after being completely split.
        public override List<Item> SplitDepletedItems => new List<Item>
        {
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Pot)
        };
    }
}