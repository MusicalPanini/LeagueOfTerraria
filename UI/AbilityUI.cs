using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using TerraLeague;
using Terraria.UI;
using System;
using Terraria.ID;
using System.Linq;
using Terraria.Graphics;
using System.Collections.Generic;
using TerraLeague.Items.Weapons;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Common.ModSystems;

namespace TerraLeague.UI
{
    internal class AbilityUI : UIState
    {
        public AbilityPanel AbilityPanel;
        public static bool visible = true;

        public override void OnInitialize()
        {
            AbilityPanel = new AbilityPanel();
            Append(AbilityPanel);

            Recalculate();
        }
    }

    public class AbilityPanel : UIMoveable
    {
        public override UIAnchor Anchor => Config.abilityUIAnchor;
        public override ref int GetXOffset => ref Config.abilityUIXOffset;
        public override ref int GetYOffset => ref Config.abilityUIYOffset;

        int abilityMargin { get { return 3; } }
        int panelPaddingLeft { get { return 2; } }
        int panelPaddingTop { get { return 5; } }
        int abilityDimention { get { return 44; } }
        int abilityWidth { get { return (abilityMargin * 2) + abilityDimention; } }

        AbilitySlotUI[] abilitySlots = new AbilitySlotUI[4];

        public override void OnInitialize()
        {
            for (int i = 0; i < abilitySlots.Length; i++)
            {
                abilitySlots[i] = new AbilitySlotUI(abilityWidth * i, 0, abilityDimention, (AbilityType)i);
                Append(abilitySlots[i]);
            }

            Width.Set((abilityWidth + abilitySlots.Length), 0);
            Height.Set(abilityWidth, 0);

            base.OnInitialize();
        }

        public override void Update(GameTime gameTime)
        {
            Width.Set((abilityWidth * abilitySlots.Length), 0);
            Height.Set(abilityWidth, 0);
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            for (int i = 0; i < abilitySlots.Length; i++)
            {
                if (abilitySlots[i].IsMouseHovering)
                    SetToolTip(modPlayer.Abilities[i], abilitySlots[i].abilityType);
            }

            base.Update(gameTime);
        }

        void SetToolTip(Ability ability, AbilityType type)
        {
            if (ability != null)
            {
                List<string> tooltip = new List<string>
                {
                    LeagueTooltip.CreateColorString(TerraLeague.TooltipHeadingColor, ability.GetAbilityName())
                };
                if (ability.GetDamageTooltip(Main.LocalPlayer) != "")
                    tooltip.AddRange(ability.GetDamageTooltip(Main.LocalPlayer).Split('\n'));
                tooltip.AddRange(ability.GetAbilityTooltip().Split('\n'));
                tooltip.Add(ability.GetCooldown() + " second cooldown");

                ToolTipUI.SetText(tooltip.ToArray());
            }
        }
    }

    public class AbilitySlotUI : UIElement
    {
        Texture2D nullImage;
        Texture2D texture_clear;
        Texture2D texture_specialCast;
        Texture2D texture_oom;
        Texture2D texture_cantCast;
        Texture2D texture_background;
        readonly public AbilityType abilityType;
        readonly UIText slotNum;
        readonly UIText slotMana;
        readonly UIText slotCD;
        readonly UIImage slotIcon;
        readonly UIImage slotOOM;
        readonly UIImage slotSpecialCast;

        public AbilitySlotUI(int left, int top, int length, AbilityType type)
        {
            abilityType = type;
            nullImage = ModContent.Request<Texture2D>("TerraLeague/AbilityImages/Template").Value;
            texture_clear = ModContent.Request<Texture2D>("TerraLeague/AbilityImages/Clear").Value;

            Left.Set(left, 0);
            Top.Set(top, 0);
            Width.Set(length, 0);
            Height.Set(length, 0);

            slotIcon = new UIImage(nullImage);
            slotIcon.Left.Pixels = 6f;
            slotIcon.Top.Pixels = 6;
            Append(slotIcon);

            slotOOM = new UIImage(texture_clear);
            slotOOM.Left.Pixels = 6f;
            slotOOM.Top.Pixels = 6;
            Append(slotOOM);

            slotSpecialCast = new UIImage(texture_clear);
            slotSpecialCast.Left.Pixels = 6f;
            slotSpecialCast.Top.Pixels = 6;
            Append(slotSpecialCast);

            slotNum = new UIText(((int)abilityType + 1).ToString(), 0.75f);
            slotNum.Left.Pixels = 2;
            slotNum.Top.Pixels = 30;
            Append(slotNum);

            slotMana = new UIText("", 0.75f);
            slotMana.Left.Pixels = 22;
            slotMana.Top.Pixels = 0;
            slotMana.TextColor = Color.CornflowerBlue;
            Append(slotMana);

            slotCD = new UIText("", 1);
            slotCD.Left.Pixels = 15;
            slotCD.Top.Pixels = 12;
            Append(slotCD);

        }

        public override void Update(GameTime gameTime)
        {
            if (GetIfAbilityExists(abilityType))
            {
                slotIcon.SetImage(GetImage(abilityType));
                slotMana.SetText(GetCost(abilityType));
                slotIcon.ImageScale = 1;
            }
            else
            {
                slotIcon.SetImage(nullImage);
                slotOOM.SetImage(texture_clear);
                slotSpecialCast.SetImage(texture_clear);
                slotIcon.ImageScale = 0;
                slotMana.SetText("");
            }

            Recalculate();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            TerraLeague.GetTextureIfNull(ref texture_background, "TerraLeague/UI/AbilityBorder");
            TerraLeague.GetTextureIfNull(ref nullImage, "TerraLeague/AbilityImages/Template");
            TerraLeague.GetTextureIfNull(ref texture_specialCast, "TerraLeague/AbilityImages/SpecialCast");
            TerraLeague.GetTextureIfNull(ref texture_oom, "TerraLeague/AbilityImages/OOM");
            TerraLeague.GetTextureIfNull(ref texture_cantCast, "TerraLeague/AbilityImages/CantCast");
            TerraLeague.GetTextureIfNull(ref texture_clear, "TerraLeague/AbilityImages/Clear");

            CalculatedStyle dimensions = GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            Main.spriteBatch.Draw(texture_background, new Rectangle(point1.X, point1.Y, width, height), Color.White);

            

            string slotNumText = "N/A";
            switch (abilityType)
            {
                case AbilityType.Q:
                    slotNumText = LeagueTooltip.ConvertKeyString(TerraLeague.QAbility);
                    break;
                case AbilityType.W:
                    slotNumText = LeagueTooltip.ConvertKeyString(TerraLeague.WAbility);
                    break;
                case AbilityType.E:
                    slotNumText = LeagueTooltip.ConvertKeyString(TerraLeague.EAbility);
                    break;
                case AbilityType.R:
                    slotNumText = LeagueTooltip.ConvertKeyString(TerraLeague.RAbility);
                    break;
                default:
                    break;
            }
            slotNum.SetText(slotNumText);

            slotCD.SetText(GetCooldown(abilityType));
            slotCD.Left.Pixels = 0;
            slotCD.Top.Pixels = 12;
            slotCD.Width.Pixels = this.Width.Pixels;
            slotCD.HAlign = 0.5f;

            if (GetIfAbilityExists(abilityType))
            {
                PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

                slotIcon.SetImage(GetImage(abilityType));

                if (modPlayer.Abilities[(int)abilityType].GetScaledManaCost() > Main.LocalPlayer.statMana)
                    slotOOM.SetImage(texture_oom);
                else if(!Ability.CheckIfNotOnCooldown(Main.LocalPlayer, abilityType) || !modPlayer.Abilities[(int)abilityType].CanCurrentlyBeCast(Main.LocalPlayer))
                    slotOOM.SetImage(texture_cantCast);
                else
                    slotOOM.SetImage(texture_clear);

                if (modPlayer.Abilities[(int)abilityType].CurrentlyHasSpecialCast(Main.LocalPlayer))
                    slotSpecialCast.SetImage(texture_specialCast);
                else
                    slotSpecialCast.SetImage(texture_clear);


                slotMana.SetText(GetCost(abilityType));
                slotIcon.ImageScale = 1;
            }
            else
            {
                slotIcon.SetImage(nullImage);
                slotOOM.SetImage(texture_clear);
                slotSpecialCast.SetImage(texture_clear);
                slotIcon.ImageScale = 0;
                slotMana.SetText("");
            }

            Recalculate();
            base.Draw(spriteBatch);
        }

        string GetCost(AbilityType type)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            try
            {
                if (modPlayer.Abilities[(int)type].GetScaledManaCost() > 0)
                    return modPlayer.Abilities[(int)type].GetScaledManaCost().ToString();
                else
                    return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        Texture2D GetImage(AbilityType type)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            try
            {
                return ModContent.Request<Texture2D>(modPlayer.Abilities[(int)type].GetIconTexturePath()).Value;
            }
            catch (Exception)
            {
                return ModContent.Request<Texture2D>("TerraLeague/AbilityImages/Template").Value;
            }
        }

        string GetCooldown(AbilityType type)
        {
            float cooldown = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().AbilityCooldowns[(int)type];

            if (cooldown > 0)
            {
                if (cooldown > 10 * 60)
                {
                    return (cooldown / 60).ToString().Split('.')[0];
                }
                else
                {
                    string text = (Math.Round(cooldown / 60, 1)).ToString();
                    if (text.Length == 1)
                        text += ".0";
                    return text;
                }
            }
            else
            {
                return "";
            }
        }

        bool GetIfAbilityExists(AbilityType type)
        {
            if (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().Abilities[(int)type] != null)
                return true;
            else
                return false;
        }
    }
}
