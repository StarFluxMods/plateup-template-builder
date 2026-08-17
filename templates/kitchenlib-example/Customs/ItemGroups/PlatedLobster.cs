using System.Collections.Generic;
using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMyMod.Customs.Items;
using UnityEngine;

namespace KitchenMyMod.Customs.ItemGroups
{
    public class PlatedLobster : CustomItemGroup<ItemGroupView>
    {
        // UniqueNameID - This is used internally to generate the ID of this GDO. Once you've set it, don't change it.
        public override string UniqueNameID => "PlatedLobster";
        
        // Prefab - This is the GameObject used for this Item's visual. AssignMaterialsByNames() is a helper method that assigns materials to the GameObject based on the names of the materials.
        public override GameObject Prefab => Mod.Bundle.LoadAsset<GameObject>("Plated Lobster").AssignMaterialsByNames();
        
        // DisposesTo - What this Item turns into when interacted with a bin.
        public override Item DisposesTo => (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate);

        // DirtiesTo - This is the what the Item turns into after the customer has completed eating.
        public override Item DirtiesTo => (Item)GDOUtils.GetExistingGDO(ItemReferences.PlateDirty);

        // ItemValue - This is how much money the earned when serving this Item.
        public override ItemValue ItemValue => ItemValue.Medium;

        // CanContainSide - When TRUE this ItemGroup can contain a Side Dish.
        public override bool CanContainSide => true;

        // Sets - Sets are the Items which make up an ItemGroup.
        public override List<ItemGroup.ItemSet> Sets => new List<ItemGroup.ItemSet>
        {
            // An ItemSets are collections of Items which are required to make this ItemGroup.
            new ItemGroup.ItemSet
            {
                // Items - The Items required to make complete this ItemSet.
                Items = new List<Item>
                {
                    // GDOUtils.GetExistingGDO(ItemReferences.Plate) - This is a helper method that gets a reference to a vanilla Item.
                    (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate)
                },
                // Min - The minimum number of Items required to complete this ItemSet.
                // Max - The maximum number of Items required to complete this ItemSet.
                Min = 1,
                Max = 1,
                
                // IsMandatory - When TRUE this ItemSet is required to complete the ItemGroup.
                IsMandatory = true
            },
            new ItemGroup.ItemSet
            {
                Items = new List<Item>
                {
                    // GDOUtils.GetExistingGDO(ItemReferences.Lobster) - This is a helper method that gets a reference to a modded Item.
                    (Item)GDOUtils.GetCustomGameDataObject<CookedLobster>().GameDataObject
                },
                Min = 1,
                Max = 1
            }
        };
    }
}