using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TerraLeague.UI;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace TerraLeague.Common.ModSystems
{
    public class UISystem : ModSystem
    {
        //static internal StatUI statUI;
        static internal ItemUI itemUI;
        static internal AbilityUI abilityUI;
        static internal CustomResourceUI healthbarUI;
        static internal ToolTipUI tooltipUI;
        static internal TeleportUI teleportUI;
        static internal PlayerUI playerUI;

        //private UserInterface userInterface1;
        private UserInterface userInterface2;
        private UserInterface userInterface3;
        private UserInterface PlayerInterface;
        public UserInterface HealthbarInterface;
        public UserInterface tooltipInterface;
        public UserInterface teleportInterface;
        public static bool StopHealthandManaText = true;

        public override void OnModLoad()
        {
            //userInterface1 = new UserInterface();
            //statUI = new StatUI();
            //StatUI.visible = 1;
            //userInterface1.SetState(statUI);
            if (Main.netMode != Terraria.ID.NetmodeID.Server)
            {
                userInterface2 = new UserInterface();
                itemUI = new ItemUI();
                ItemUI.visible = true;
                userInterface2.SetState(itemUI);

                userInterface3 = new UserInterface();
                abilityUI = new AbilityUI();
                AbilityUI.visible = true;
                userInterface3.SetState(abilityUI);

                HealthbarInterface = new UserInterface();
                healthbarUI = new CustomResourceUI();
                CustomResourceUI.visible = true;
                HealthbarInterface.SetState(healthbarUI);

                tooltipInterface = new UserInterface();
                tooltipUI = new ToolTipUI();
                //ToolTipUI.visible = true;
                tooltipInterface.SetState(tooltipUI);

                teleportInterface = new UserInterface();
                teleportUI = new TeleportUI();
                TeleportUI.visible = false;
                teleportInterface.SetState(teleportUI);

                PlayerInterface = new UserInterface();
                playerUI = new PlayerUI();
                PlayerInterface.SetState(playerUI);
            }
            base.OnModLoad();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (TerraLeague.ToggleStats.JustReleased)
            {
                ItemUI.extraStats = !ItemUI.extraStats;
            }

            base.UpdateUI(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourseBar = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));

            if (TerraLeague.UseModResourceBar)
            {
                if (resourseBar < 0)
                    resourseBar = 7;
                else
                {
                    layers[resourseBar].Active = false;
                    //layers.RemoveAt(resourseBar);
                }
                layers.Insert(resourseBar, new LegacyGameInterfaceLayer("TerraLeague: Resource Bar",
                delegate
                {
                    if (CustomResourceUI.visible)
                    {
                        HealthbarInterface.Update(Main._drawInterfaceGameTime);
                        healthbarUI.Draw(Main.spriteBatch);
                    }
                    return true;
                },
                InterfaceScaleType.UI));
            }

            int mousetextLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mousetextLayer < 0)
                mousetextLayer = 36;
            layers.Insert(mousetextLayer, new LegacyGameInterfaceLayer("TerraLeague: Custom Mouse Text",
                delegate
                {
                    if (ToolTipUI.visible)
                    {
                        tooltipInterface.Update(Main._drawInterfaceGameTime);
                        tooltipUI.Draw(Main.spriteBatch);
                    }
                    return true;
                },
                InterfaceScaleType.UI));

            int inventoryLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            if (inventoryLayer < 0)
                inventoryLayer = 4;

            //layers.Insert(inventoryLayer, new LegacyGameInterfaceLayer(
            //"TerraLeague: Stat Hud",
            //delegate
            //{
            //    if (StatUI.visible < 0)
            //    {
            //        userInterface1.Update(Main._drawInterfaceGameTime);
            //        statUI.Draw(Main.spriteBatch);
            //    }
            //    return true;
            //},
            //InterfaceScaleType.UI));

            layers.Insert(0, new LegacyGameInterfaceLayer(
            "TerraLeague: Player Hud",
            delegate
            {
                PlayerInterface.Update(Main._drawInterfaceGameTime);
                playerUI.Draw(Main.spriteBatch);
                return true;
            },
            InterfaceScaleType.Game));
            layers.Insert(inventoryLayer, new LegacyGameInterfaceLayer(
            "TerraLeague: Item Hud",
            delegate
            {
                if (ItemUI.visible)
                {
                    userInterface2.Update(Main._drawInterfaceGameTime);
                    itemUI.Draw(Main.spriteBatch);
                }
                return true;
            },
            InterfaceScaleType.UI));

            layers.Insert(resourseBar, new LegacyGameInterfaceLayer(
            "TerraLeague: Ability Hud",
            delegate
            {
                if (AbilityUI.visible)
                {
                    userInterface3.Update(Main._drawInterfaceGameTime);
                    abilityUI.Draw(Main.spriteBatch);
                }
                return true;
            },
            InterfaceScaleType.UI));

            layers.Insert(resourseBar, new LegacyGameInterfaceLayer(
            "TerraLeague: Teleport UI",
            delegate
            {
                if (TeleportUI.visible)
                {
                    teleportInterface.Update(Main._drawInterfaceGameTime);
                    teleportUI.Draw(Main.spriteBatch);
                }
                return true;
            },
            InterfaceScaleType.UI));

            //layers.RemoveAll(layer => layer.Name.Equals("Vanilla: Interface Logic 2"));
        }


        public static void HealthAndManaHitBoxes()
        {
            if (!StopHealthandManaText)
            {
                return;
            }

            bool isHealthOver200 = (Main.LocalPlayer.statLifeMax2 > 200);
            int heartWidthTotal = isHealthOver200 ? 260 : (26 * Main.player[Main.myPlayer].statLifeMax2 / 20);

            int healthBarX = 500 + (Main.screenWidth - 800);
            int healthBarY = 32;
            int healthBarWidth = 500 + heartWidthTotal + (Main.screenWidth - 800);
            int healthBarHeight = isHealthOver200 ? TextureAssets.Heart.Height() + 32 : 32;

            Rectangle healthBar = new Rectangle(healthBarX, healthBarY, healthBarWidth, healthBarHeight);


            int manaBarX = 762 + (Main.screenWidth - 800);
            int manaBarY = 30;
            int manaBarHeight = 28 * Main.LocalPlayer.statManaMax2 / 20;
            int manaBarWidth = TextureAssets.Mana.Width() + 2;

            Rectangle manaBar = new Rectangle(manaBarX, manaBarY, manaBarWidth, manaBarHeight);

            StopHealthManaMouseOver(healthBar, manaBar);
        }

        public static void StopHealthManaMouseOver(Rectangle HealthHitBox, Rectangle ManaHitBox)
        {
            Main.mouseText = HealthHitBox.Contains(Main.mouseX, Main.mouseY) ||
                ManaHitBox.Contains(Main.mouseX, Main.mouseY);
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            HealthAndManaHitBoxes();
        }
    }
}
