﻿using SMLHelper.V2.Json;
using SMLHelper.V2.Options;
using SMLHelper.V2.Options.Attributes;
using System.Collections.Generic;

namespace InventoryColorCustomization
{
    // worst class i've written since the pre-ECC gargantuan leviathan
    public class Options : ModOptions
    {
        private SaveOptions savedOptions;

        public Options() : base("Inventory Color Customization (Requires restart!)")
        {
            ToggleChanged += OnToggleChanged;
            ChoiceChanged += OnChoiceChanged;
            savedOptions = new SaveOptions();
            savedOptions.Load(true);
        }

        public override void BuildModOptions()
        {
            AddBackgroundColorOption(new BackgroundType(CraftData.BackgroundType.Normal), "Normal Item Color");
            AddBackgroundColorOption(new BackgroundType(BackgroundTypeManager.Category_Creatures), "Creatures Color");
            AddBackgroundColorOption(new BackgroundType(CraftData.BackgroundType.ExosuitArm), "Exosuit Arm Color");
            AddBackgroundColorOption(new BackgroundType(CraftData.BackgroundType.PlantWater), "Water Plant Color");
            AddBackgroundColorOption(new BackgroundType(BackgroundTypeManager.Category_Precursor), "Precursor Items Color");
            AddBackgroundColorOption(new BackgroundType(CraftData.BackgroundType.PlantAir), "Land Plant Color");
            AddBackgroundColorOption(new BackgroundType(CraftData.BackgroundType.PlantWaterSeed), "Water Seed Color");
            AddBackgroundColorOption(new BackgroundType(CraftData.BackgroundType.PlantAirSeed), "Land Seed Color");
            AddToggleOption("TransparentBackgrounds", "Transparent Backgrounds", savedOptions.TransparentBackgrounds);
            AddToggleOption("SquareIcons", "Use Square Icons", savedOptions.SquareIcons);

            // AddBackgroundColorOption(CraftData.BackgroundType.Blueprint, "Blueprint Color (Unused)");
        }

        public void OnToggleChanged(object sender, ToggleChangedEventArgs eventArgs)
        {
            switch (eventArgs.Id)
            {
                case "TransparentBackgrounds":
                    savedOptions.TransparentBackgrounds = eventArgs.Value;
                    break;
                case "SquareIcons":
                    savedOptions.SquareIcons = eventArgs.Value;
                    break;
            }
            savedOptions.Save();
        }

        public void OnChoiceChanged(object sender, ChoiceChangedEventArgs eventArgs)
        {
            var key = eventArgs.Id;
            int value = ColorChoiceManager.ChoiceNameToIndex(key, eventArgs.Value);
            if (savedOptions.BackgroundColorChoices == null)
            {
                savedOptions.BackgroundColorChoices = new Dictionary<string, int>();
            }
            var dict = savedOptions.BackgroundColorChoices;
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
            savedOptions.Save();
        }

        private void AddBackgroundColorOption(BackgroundType backgroundType, string label)
        {
            string id = backgroundType.GetData().ID;
            var choices = ColorChoiceManager.GetColorChoiceNames(backgroundType);
            AddChoiceOption(id, label, choices, savedOptions.GetBackgroundColorChoice(BackgroundDataManager.GetBackgroundData(backgroundType).ID));
        }

        public int GetSelectedIndexForBackground(string backgroundType)
        {
            return savedOptions.GetBackgroundColorChoice(backgroundType);
        }

        public bool TransparentBackgrounds { get { return savedOptions.TransparentBackgrounds; } }

        public bool SquareIcons { get { return savedOptions.SquareIcons; } }

    }
}