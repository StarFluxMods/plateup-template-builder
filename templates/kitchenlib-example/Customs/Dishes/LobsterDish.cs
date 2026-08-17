using System.Collections.Generic;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMyMod.Customs.ItemGroups;
using KitchenMyMod.Customs.Items;
using UnityEngine;

namespace KitchenMyMod.Customs.Dishes
{
    public class LobsterDish : CustomDish
    {
        // UniqueNameID - This is used internally to generate the ID of this GDO. Once you've set it, don't change it.
        public override string UniqueNameID => "LobsterDish";

        // ExpReward - Determines how much XP this Unlock provides.
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.Medium;

        // IsUnlockable - When TRUE this Unlock can appear in the card selector.
        public override bool IsUnlockable => true;

        // UnlockGroup - Determines what type of Unlock this is.
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;

        // CardType - Determines when this Unlock can be selected.
        public override CardType CardType => CardType.Default;

        // CustomerMultiplier - Determines the customer difference this Unlock provides.
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.SmallDecrease;

        // Type - This is used to decide what phase this Dish should be ordered.
        public override DishType Type => DishType.Base;

        // Difficulty - This is displayed in the lobby. (0 - 5)
        public override int Difficulty => 1;

        // StartingNameSet - The list of names used to decide the default Restaurant name.
        public override List<string> StartingNameSet => new List<string>
        {
            "Claws and Effect",
            "Pinch Me, I'm Dreaming",
            "Shell Yeah!",
            "Claw-some Bites",
            "Shell Shocked",
            "Crabby Attitude",
            "Clawfully Good"
        };

        // MinimumIngredients - The ingredients required to make this Dish.
        public override HashSet<Item> MinimumIngredients => new HashSet<Item>()
        {
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Pot),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Water),
            (Item)GDOUtils.GetCustomGameDataObject<RawLobster>().GameDataObject,
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate)
        };

        // RequiredProcesses - The processes required to make this Dish.
        public override HashSet<Process> RequiredProcesses => new HashSet<Process>
        {
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.Cook)
        };

        // IconPrefab - This is the Icon displayed in the lobby.
        public override GameObject IconPrefab => Mod.Bundle.LoadAsset<GameObject>("Lobster Dish Icon").AssignMaterialsByNames();

        // ResultingMenuItems - What menu Items are available to customers after unlocking this Dish.
        public override List<Dish.MenuItem> ResultingMenuItems => new List<Dish.MenuItem>
        {
            new Dish.MenuItem
            {
                Item = (Item)GDOUtils.GetCustomGameDataObject<PlatedLobster>().GameDataObject,
                Phase = MenuPhase.Main,
                Weight = 1,
                DynamicMenuType = DynamicMenuType.Static,
                DynamicMenuIngredient = null
            }
        };

        // IsAvailableAsLobbyOption - When TRUE this Dish will appear in the lobby.
        public override bool IsAvailableAsLobbyOption => true;

        // Recipe - This is the recipe displayed when unlocking this Dish.
        public override Dictionary<Locale, string> Recipe => new Dictionary<Locale, string>
        {
            { Locale.English, "Add raw lobster and water into a pot to cook. Portion onto a plate and serve!" }
        };

        // InfoList - This is used to assign localisation to this Dish.
        public override List<(Locale, UnlockInfo)> InfoList => new List<(Locale, UnlockInfo)>
        {
            (Locale.English, new UnlockInfo
            {
                Name = "Lobster",
                Description = "Adds Lobster as a Main",
                FlavourText = ""
            })
        };
    }
}