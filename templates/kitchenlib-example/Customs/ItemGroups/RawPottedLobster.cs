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
    public class RawPottedLobster : CustomItemGroup<ItemGroupView>
    {
        // UniqueNameID - This is used internally to generate the ID of this GDO. Once you've set it, don't change it.
        public override string UniqueNameID => "RawPottedLobster";
        
        // Prefab - This is the GameObject used for this Item's visual. AssignMaterialsByNames() is a helper method that assigns materials to the GameObject based on the names of the materials.
        public override GameObject Prefab => Mod.Bundle.LoadAsset<GameObject>("Raw Potted Lobster").AssignMaterialsByNames();
        
        // DisposesTo - What this Item turns into when interacted with a bin.
        public override Item DisposesTo => (Item)GDOUtils.GetExistingGDO(ItemReferences.Pot);
        
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
                    (Item)GDOUtils.GetExistingGDO(ItemReferences.Pot)
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
                    (Item)GDOUtils.GetCustomGameDataObject<RawLobster>().GameDataObject,
                    (Item)GDOUtils.GetExistingGDO(ItemReferences.Water)
                },
                Min = 2,
                Max = 2
            }
        };
        
        // Processes - These are the Processes which can be applied to this Item.
        public override List<Item.ItemProcess> Processes => new List<Item.ItemProcess>
        {
            new Item.ItemProcess
            {
                Process = (Process)GDOUtils.GetExistingGDO(ProcessReferences.Cook),
                Duration = 5,
                Result = (Item)GDOUtils.GetCustomGameDataObject<CookedPottedLobster>().GameDataObject
            }
        };
        
        // OnRegister - This is called when a GameDataObject is registered.
        public override void OnRegister(ItemGroup gameDataObject)
        {
            base.OnRegister(gameDataObject);
            
            // This gets the ItemGroupView component from the Prefab. Unless modified otherwise, this component is automatically added to all ItemGroup Prefabs.
            ItemGroupView view = gameDataObject.Prefab.GetComponent<ItemGroupView>();

            // This is used to render the correct GameObjects based on the Items in the ItemSets.
            view.ComponentGroups = new List<ItemGroupView.ComponentGroup>
            {
                new ItemGroupView.ComponentGroup
                {
                    Item = (Item)GDOUtils.GetExistingGDO(ItemReferences.Pot),
                    // GameObjectUtils.GetChildObject() is a helper method that gets a child GameObject from the Prefab. The first argument is the parent GameObject, the second is the path to the child GameObject.
                    GameObject = GameObjectUtils.GetChildObject(gameDataObject.Prefab, "Pot/Pot_1"),
                    DrawAll = true
                },
                new ItemGroupView.ComponentGroup
                {
                    Item = (Item)GDOUtils.GetExistingGDO(ItemReferences.Water),
                    GameObject = GameObjectUtils.GetChildObject(gameDataObject.Prefab, "Pot/Water"),
                    DrawAll = true
                },
                new ItemGroupView.ComponentGroup
                {
                    Item = (Item)GDOUtils.GetCustomGameDataObject<RawLobster>().GameDataObject,
                    GameObject = GameObjectUtils.GetChildObject(gameDataObject.Prefab, "Raw Lobster"),
                    DrawAll = true
                }
            };
        }
    }
}