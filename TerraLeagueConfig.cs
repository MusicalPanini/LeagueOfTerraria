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
using TerraLeague.Common.ModSystems;
using TerraLeague.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;

namespace TerraLeague
{
    [BackgroundColor(4, 74, 26)]
    [Label("UI Config")]
    public class TerraLeagueUIConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        [BackgroundColor(75, 75, 75)]
        [Label("Lock Mod UI")]
        [Tooltip("Locks the UI in place, preventing it from being moved with right click and screen resizing")]
        public bool lockUI;

        [DefaultValue(true)]
        [BackgroundColor(19, 122, 113)]
        [Label("Enable custom resource and buff UI")]
        [Tooltip("WARNING: You wont be able to clearly see shield values with this off")]
        public bool UseModResourceBar;


        // ============= [Item UI] ============= \\
        [Header("Item UI")]

        [DefaultValue(338)]
        [BackgroundColor(137, 129, 58)]
        [Range(-9999, 9999)]
        [Label("Item UI Anchor X Offset")]
        public int itemUIXOffset;

        [DefaultValue(64)]
        [BackgroundColor(137, 129, 58)]
        [Range(-9999, 9999)]
        [Label("Item UI Anchor Y Offset")]
        public int itemUIYOffset;

        [DefaultValue(UIAnchor.BottomCenter)]
        [BackgroundColor(137, 129, 58)]
        [Label("Item UI Anchor Point")]
        public UIAnchor itemUIAnchor;


        // ============= [Summoner UI] ============= \\
        [Header("Summoner Spell UI")]

        [DefaultValue(196)]
        [BackgroundColor(137, 129, 58)]
        [Range(-9999, 9999)]
        [Label("Summoner UI Anchor X Offset")]
        public int sumUIXOffset;

        [DefaultValue(64)]
        [BackgroundColor(137, 129, 58)]
        [Range(-9999, 9999)]
        [Label("Summoner UI Anchor Y Offset")]
        public int sumUIYOffset;

        [DefaultValue(UIAnchor.BottomCenter)]
        [BackgroundColor(137, 129, 58)]
        [Label("Summoner UI Anchor Point")]
        public UIAnchor sumUIAnchor;


        // ============= [Stat UI] ============= \\
        [Header("Stat UI")]

        [DefaultValue(196)]
        [BackgroundColor(137, 129, 58)]
        [Range(-9999, 9999)]
        [Label("Stat UI Anchor X Offset")]
        public int statUIXOffset;

        [DefaultValue(42)]
        [BackgroundColor(137, 129, 58)]
        [Range(-9999, 9999)]
        [Label("Stat UI Anchor Y Offset")]
        public int statUIYOffset;

        [DefaultValue(UIAnchor.BottomLeft)]
        [BackgroundColor(137, 129, 58)]
        [Label("Stat UI Anchor Point")]
        public UIAnchor statUIAnchor;

        // ============= [Ability UI] ============= \\
        [Header("Ability UI")]

        [DefaultValue(0)]
        [BackgroundColor(137, 129, 58)]
        [Range(-9999, 9999)]
        [Label("Ability UI Anchor X Offset")]
        public int abilityUIXOffset;

        [DefaultValue(64)]
        [BackgroundColor(137, 129, 58)]
        [Range(-9999, 9999)]
        [Label("Ability UI Anchor Y Offset")]
        public int abilityUIYOffset;

        [DefaultValue(UIAnchor.BottomCenter)]
        [BackgroundColor(137, 129, 58)]
        [Label("Ability UI Anchor Point")]
        public UIAnchor abilityUIAnchor;

        // ============= [Resource UI] ============= \\
        [Header("Resource Bar UI")]

        [DefaultValue(0)]
        [BackgroundColor(137, 129, 58)]
        [Range(-9999, 9999)]
        [Label("Resource UI Anchor X Offset")]
        public int resourceUIXOffset;

        [DefaultValue(114)]
        [BackgroundColor(137, 129, 58)]
        [Range(-9999, 9999)]
        [Label("Resource UI Anchor Y Offset")]
        public int resourceUIYOffset;

        [DefaultValue(UIAnchor.BottomCenter)]
        [BackgroundColor(137, 129, 58)]
        [Label("Resource UI Anchor Point")]
        public UIAnchor resourceUIAnchor;

        [Increment(5)]
        [Range(20, 100)]
        [DefaultValue(50)]
        [Slider]
        [BackgroundColor(186, 33, 55)]
        [Label("How much life per marker on the health bar")]
        public int healthBarDividerSpacing;

        [Increment(5)]
        [Range(20, 100)]
        [DefaultValue(50)]
        [Slider]
        [BackgroundColor(51, 106, 183)]
        [Label("How much mana per marker on the mana bar")]
        public int manaBarDividerSpacing;

        // ============= [Buff UI] ============= \\
        [Header("Buff UI")]

        [DefaultValue(0)]
        [BackgroundColor(137, 129, 58)]
        [Range(-9999, 9999)]
        [Label("Buff UI Anchor X Offset")]
        public int buffUIXOffset;

        [DefaultValue(164)]
        [BackgroundColor(137, 129, 58)]
        [Range(-9999, 9999)]
        [Label("Buff UI Anchor Y Offset")]
        public int buffUIYOffset;

        [DefaultValue(UIAnchor.BottomCenter)]
        [BackgroundColor(137, 129, 58)]
        [Label("Buff UI Anchor Point")]
        public UIAnchor buffUIAnchor;

        [DefaultValue(true)]
        [BackgroundColor(137, 129, 58)]
        [Label("Use small icons")]
        public bool buffUISmall;

        [DefaultValue(false)]
        [BackgroundColor(137, 129, 58)]
        [Label("Icons are slightly transparent")]
        public bool buffTransparent;

        [DefaultValue(BuffUIDim._1x24)]
        [BackgroundColor(137, 129, 58)]
        [Label("Buff Display Dimentions")]
        public BuffUIDim buffUIDimentions;

        public override void OnChanged()
        {
            if (UISystem.itemUI != null)
            {
                //UISystem.itemUI.ItemPanel.Anchor = itemUIAnchor;
                UIMoveable.SetPosition(UISystem.itemUI.ItemPanel.Anchor, itemUIXOffset, itemUIYOffset, UISystem.itemUI.ItemPanel);

                //UISystem.itemUI.StatPanel.Anchor = statUIAnchor;
                UIMoveable.SetPosition(UISystem.itemUI.StatPanel.Anchor, statUIXOffset, statUIYOffset, UISystem.itemUI.StatPanel);

                //UISystem.itemUI.SummonerPanel.Anchor = sumUIAnchor;
                UIMoveable.SetPosition(UISystem.itemUI.SummonerPanel.Anchor, sumUIXOffset, sumUIYOffset, UISystem.itemUI.SummonerPanel);
            }

            UI.ResourceBar.healthBarDividerDistance = healthBarDividerSpacing;
            UI.ResourceBar.manaBarDividerDistance = manaBarDividerSpacing;
            TerraLeague.UseModResourceBar = UseModResourceBar;
            TerraLeague.LockUI = lockUI;
            base.OnChanged();
        }
    }

    [BackgroundColor(4, 74, 26)]
    [Label("Client Config")]
    public class TerraLeagueClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

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
            TerraLeague.fogIntensity = drawMist;
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
