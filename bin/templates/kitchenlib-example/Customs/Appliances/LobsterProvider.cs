using System.Collections.Generic;
using Kitchen;
using Kitchen.Components;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using KitchenMyMod.Customs.Items;
using UnityEngine;

namespace KitchenMyMod.Customs.Appliances
{
    public class LobsterProvider : CustomAppliance
    {
        // UniqueNameID - This is used internally to generate the ID of this GDO. Once you've set it, don't change it.
        public override string UniqueNameID => "LobsterProvider";
        
        // Prefab - This is the GameObject used for this Appliance's visual. AssignMaterialsByNames() is a helper method that assigns materials to the GameObject based on the names of the materials.
        public override GameObject Prefab => Mod.Bundle.LoadAsset<GameObject>("Lobster Provider").AssignMaterialsByNames();
        
        // Properties - The Properties attached to the Appliance.
        public override List<IApplianceProperty> Properties => new List<IApplianceProperty>
        {
            KitchenPropertiesUtils.GetUnlimitedCItemProvider(GDOUtils.GetCustomGameDataObject<RawLobster>().ID)
        };

        // PriceTier - This determines the default price of the Appliance in the shop.
        public override PriceTier PriceTier => PriceTier.Medium;
        
        // RarityTier - This determines the color of the Blueprint outline.
        public override RarityTier RarityTier => RarityTier.Uncommon;
        
        // IsPurchasable - When TRUE this Appliance can appear in the shop.
        public override bool IsPurchasable => true;
        
        // SellOnlyAsDuplicate - When TRUE this Appliance will only appear in the shop if already owned. 
        public override bool SellOnlyAsDuplicate => true;

        // InfoList - This is used to assign localisation to this Appliance.
        public override List<(Locale, ApplianceInfo)> InfoList => new List<(Locale, ApplianceInfo)>
        {
            (Locale.English, new ApplianceInfo
            {
                Name = "Raw Lobster",
                Description = "Provides Raw Lobster"
            })
        };

        // OnRegister - This is called when a GameDataObject is registered.
        public override void OnRegister(Appliance gameDataObject)
        {
            base.OnRegister(gameDataObject);

            // AnimationSoundSource - This is used to play a sound when the Appliance is interacted with.
            AnimationSoundSource soundSource = gameDataObject.Prefab.GetChild("Locker/Locker").TryAddComponent<AnimationSoundSource>();
            soundSource.SoundList = new List<AudioClip>() { Mod.Bundle.LoadAsset<AudioClip>("Fridge_mixdown") };
            soundSource.Category = SoundCategory.Effects;
            soundSource.ShouldLoop = false;
            
            // ItemSourceView - This is used to display the Item provided by the Appliance, and trigger the Animation on interaction.
            ItemSourceView sourceView = gameDataObject.Prefab.TryAddComponent<ItemSourceView>();
            var quad = gameDataObject.Prefab.GetChild("Locker/Quad").GetComponent<MeshRenderer>();
            quad.materials = MaterialUtils.GetMaterialArray("Flat Image");
            ReflectionUtils.GetField<ItemSourceView>("Renderer").SetValue(sourceView, quad);
            ReflectionUtils.GetField<ItemSourceView>("Animator").SetValue(sourceView, gameDataObject.Prefab.GetChild("Locker/Locker").GetComponent<Animator>());
        }
    }
}