﻿using QModManager.API.ModLoading;
using HarmonyLib;
using SMLHelper.V2.Handlers;
using System.IO;
using System.Reflection;
using UnityEngine;
using RisingLava.Mono;

namespace RisingLava
{
    [QModCore]
    public static class Main
    {
        public static LavaConfig config = OptionsPanelHandler.Main.RegisterModOptions<LavaConfig>();

        public static Assembly assembly = Assembly.GetExecutingAssembly();

        public static AssetBundle assetBundle;

        public static Prefabs.Tools.LavaMonitor lavaMonitor;

        [QModPatch]
        public static void Entry()
        {
            assetBundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(assembly.Location), "Assets", "risinglavaassets"));

            Harmony harmony = new Harmony("Lee23.RisingLava");
            harmony.PatchAll(assembly);

            ConsoleCommandsHandler.Main.RegisterConsoleCommands(typeof(LavaCommands));

            lavaMonitor = new Prefabs.Tools.LavaMonitor();
            lavaMonitor.Patch();
        }

        public static bool EnabledInConfig
        {
            get
            {
                return config.EnableMod;
            }
        }

        public static float LavaLevel
        {
            get
            {
                var wm = LavaMove.main;
                if (wm != null)
                {
                    return wm.lavaLevel;
                }
                return config.BaseLavaLevel;
            }
        }

        public static bool AutoModeEnabled
        {
            get
            {
                if (LavaCommands.overrideAutoMode.overriden)
                {
                    return LavaCommands.overrideAutoMode.value;
                }
                return config.BeginAutomatically;
            }
        }

        public static float MaxLavaLevel
        {
            get
            {
                if (LavaCommands.overrideMaxHeight.overriden)
                {
                    return LavaCommands.overrideMaxHeight.value;
                }
                return config.LavaLevelMax;
            }
        }

        public static float LavaMoveSpeed
        {
            get
            {
                if (LavaCommands.overrideLavaSpeed.overriden)
                {
                    return LavaCommands.overrideLavaSpeed.value * 4f;
                }
                return config.LavaMoveSpeed;
            }
        }

        public static float ActualLavaMoveSpeed
        {
            get
            {
                return LavaMoveSpeed / 4f;
            }
        }

        public const float kLavaTempIncrease = 40f;
        public const float kLavaHeatHeight = 20f;
        public const float kUnderlavaTemperature = 700f;

        public const float kDistortionEffectHeight = 40f;
    }
}