using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;

namespace TerraLeague
{
    [BackgroundColor(4, 74, 26)]
    [Label("Client Config")]
    public class TerraLeagueClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("UI Options")]

        [Increment(5)]
        [Range(20, 100)]
        [DefaultValue(50)]
        [Slider]
        [BackgroundColor(186,33,55)]
        [Label("How much life per marker on the health bar")]
        public int healthBarDividerSpacing;

        [Increment(5)]
        [Range(20, 100)]
        [DefaultValue(50)]
        [Slider]
        [BackgroundColor(51, 106, 183)]
        [Label("How much mana per marker on the mana bar")]
        public int manaBarDividerSpacing;

        [DefaultValue(true)]
        [BackgroundColor(19, 122, 113)]
        [Label("Use the custom resource bar")]
        [Tooltip("WARNING: You wont be able to clearly see shield values with this off")]
        public bool UseModResourceBar;


        [Header("Gameplay Options")]

        [DefaultValue(true)]
        [BackgroundColor(51, 150, 183)]
        [Label("Use mana regen overhaul")]
        [Tooltip("WARNING: The mod was not built around the vanilla system")]
        public bool UseCustomManaRegen;

        [DefaultValue(true)]
        [BackgroundColor(100, 100, 100)]
        [Label("Convert defence into armor and resist")]
        [Tooltip("This will cause defence to not block any damage, but turn it into armor and resist instead")]
        public bool UseCustomDefenceStat;

        [DefaultValue(1)]
        [Range(0, 1)]
        [BackgroundColor(0, 96, 29)]
        [Slider]
        [Label("Set the intensity of the Black Mist effect")]
        public float drawMist;


        [Header("Debug Tools")]

        [DefaultValue(false)]
        [BackgroundColor(200, 200, 0)]
        [Label("[DEBUG] Remove Ability Cooldowns")]
        public bool noAbilityCooldowns;

        [DefaultValue(false)]
        [BackgroundColor(75, 75, 75)]
        [Label("[DEBUG] Remove Item Cooldowns")]
        public bool noItemCooldowns;

        [DefaultValue(false)]
        [BackgroundColor(200, 200, 0)]
        [Label("[DEBUG] Remove Summoner Cooldowns")]
        public bool noSummonerCooldowns;

        [DefaultValue(false)]
        [BackgroundColor(75, 75, 75)]
        [Label("[DEBUG] Display Logs")]
        public bool showLogging;

        public override void OnChanged()
        {
            UI.ResourceBar.healthBarDividerDistance = healthBarDividerSpacing;
            UI.ResourceBar.manaBarDividerDistance = manaBarDividerSpacing;
            TerraLeague.fogIntensity = drawMist;
            TerraLeague.UseModResourceBar = UseModResourceBar;
            TerraLeague.canLog = showLogging;

            if (TerraLeague.debugMode || Main.netMode == NetmodeID.SinglePlayer)
            {
                TerraLeague.noAbilityCooldowns = noAbilityCooldowns;
                TerraLeague.noItemCooldowns = noItemCooldowns;
                TerraLeague.noSummonerCooldowns = noSummonerCooldowns;
            }
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                TerraLeague.UseCustomManaRegen = UseCustomManaRegen;
                TerraLeague.UseCustomDefenceStat = UseCustomDefenceStat;
            }

            base.OnChanged();
        }
    }

    [BackgroundColor(4, 74, 26)]
    [Label("Server Config")]
    public class TerraLeagueServerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("Gameplay Options")]

        [DefaultValue(true)]
        [BackgroundColor(51, 150, 183)]
        [Label("Use mana regen overhaul for server")]
        [Tooltip("WARNING: The mod was not built around the vanilla system")]
        public bool UseCustomManaRegen;

        [DefaultValue(true)]
        [BackgroundColor(100, 100, 100)]
        [Label("Convert defence into armor and resist for server")]
        [Tooltip("This will cause defence to not block any damage, but turn it into armor and resist instead")]
        public bool UseCustomDefenceStat;


        [Header("Debug Tools")]

        [DefaultValue(false)]
        [BackgroundColor(100, 100, 100)]
        [Label("Enable clientside debug configs")]
        [Tooltip("This allows individual clients to toggle debug options")]
        public bool debugModeEnabled;

        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
        {
            //if (Main.player == NetmodeID.Server)
            //{
            //    return true;
            //}
            //else
            //{
                message = "You are not the server, you cannot change the server settings";
                return false;
            //}
        }

        public override void OnChanged()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                TerraLeague.UseCustomManaRegen = UseCustomManaRegen;
                TerraLeague.UseCustomDefenceStat = UseCustomDefenceStat;
                TerraLeague.debugMode = debugModeEnabled;

                if (!TerraLeague.debugMode)
                {
                    TerraLeague.noAbilityCooldowns = false;
                    TerraLeague.noItemCooldowns = false;
                    TerraLeague.noSummonerCooldowns = false;
                    TerraLeague.UseCustomManaRegen = false;
                    TerraLeague.UseCustomDefenceStat = false;
                }
            }

            base.OnChanged();
        }
    }
}
