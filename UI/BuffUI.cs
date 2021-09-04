using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Common.ModSystems;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace TerraLeague.UI
{
    public class BuffUI : UIState
    {
        public static bool visible = true;
        public BuffPanelUI BuffPanel;

        public override void OnInitialize()
        {
            BuffPanel = new BuffPanelUI();
            base.Append(BuffPanel);

            base.OnInitialize();
        }
    }

    public class BuffPanelUI : UIMoveable
    {
        public BuffElementUI[] BuffElements;
        public override ref int GetXOffset => ref Config.buffUIXOffset;
        public override ref int GetYOffset => ref Config.buffUIYOffset;
        public override UIAnchor Anchor => Config.buffUIAnchor;

        public Point BuffUIDimentions { get { return Dimentions[Config.buffUIDimentions]; } }
        readonly Dictionary<BuffUIDim, Point> Dimentions = new Dictionary<BuffUIDim, Point>()
        {
            { BuffUIDim._1x24, new Point(1, 24)},
            { BuffUIDim._2x12, new Point(2, 12)},
            { BuffUIDim._3x8, new Point(3, 8)},
            { BuffUIDim._4x6, new Point(4, 6)},
            { BuffUIDim._6x4, new Point(6, 4)},
            { BuffUIDim._8x3, new Point(8, 3)},
            { BuffUIDim._12x2, new Point(12, 2)},
            { BuffUIDim._24x1, new Point(24, 1)}
        };

        public override void OnInitialize()
        {
            base.OnInitialize();

            BuffElements = new BuffElementUI[22];

            for (int i = 0; i < BuffElements.Length; i++)
            {
                BuffElements[i] = new BuffElementUI(i);
                Append(BuffElements[i]);
            }
        }

        public override void Update(GameTime gameTime)
        {
            SetUIElementPositions();
            base.Update(gameTime);
        }

        public void SetUIElementPositions()
        {
            int buffElement = 0;
            Point elementDimentions = BuffUIDimentions;

            for (int y = 0; y < elementDimentions.X; y++)
            {
                for (int x = 0; x < elementDimentions.Y; x++)
                {
                    if (buffElement < BuffElements.Length)
                    {
                        BuffElementUI element = BuffElements[buffElement];

                        element.Left.Set(x * BuffElementUI.ElementWidth, 0);
                        element.Top.Set(y * BuffElementUI.ElementHeight, 0);

                        buffElement++;
                    }
                }
            }

            int X = elementDimentions.Y > 22 ? 22 : elementDimentions.Y;
            int Y = elementDimentions.X > 22 ? 22 : elementDimentions.X;
            Width.Set(X * BuffElementUI.ElementWidth, 0);
            Height.Set(Y * BuffElementUI.ElementHeight, 0);
        }
    }

    public class BuffElementUI : UIElement
    {
        UIImage buffImage;
        UIText buffTimer;
        static TerraLeagueUIConfig Config = ModContent.GetInstance<TerraLeagueUIConfig>();
        internal int BuffSlot;

        Texture2D GetBuffImage { get { return TextureAssets.Buff[BuffType].Value; } }

        static bool BuffUIIsSmall { get { return Config.buffUISmall; } }
        bool BuffTransparent { get { return Config.buffTransparent && !IsMouseHovering; } }
        float TextScale { get { return BuffUIIsSmall ? 0.75f : 0.75f; } }
        static int ElementMargin { get { return BuffUIIsSmall ? 3 : 2; } }
        public static int ElementWidth { get { return (ElementMargin * 2) + ImageDimentions; } }
        public static int ElementHeight { get { return (ElementMargin * 2) + TextHeight + ImageDimentions + ImageTextSpacing; } }
        static int ImageDimentions { get { return BuffUIIsSmall ? 16 : 32; } }
        static int TextHeight { get { return BuffUIIsSmall ? 8 : 12; } }
        static int ImageTextSpacing { get { return BuffUIIsSmall ? 0 : 2; } }

        int BuffType { get { return Main.LocalPlayer.buffType[BuffSlot]; } }
        int BuffTime { get { return Main.LocalPlayer.buffTime[BuffSlot]; } }

        public BuffElementUI(int buffslot)
        {
            BuffSlot = buffslot;
        }

        public override void OnInitialize()
        {
            buffImage = new UIImage(ModContent.Request<Texture2D>("TerraLeague/Textures/UI/BlankBuff").Value);
            Append(buffImage);

            buffTimer = new UIText("", TextScale);
            Append(buffTimer);

            UpdateImage();
            UpdateText();
            UpdateScaling();

            base.OnInitialize();
        }

        public override void Update(GameTime gameTime)
        {
            UpdateImage();
            UpdateText();
            UpdateScaling();
            SetTooltip();
            base.Update(gameTime);
        }

        void UpdateText()
        {
            if ((BuffTime / 60) == 0 || Main.buffNoTimeDisplay[BuffType])
            {
                buffTimer.SetText("", TextScale, false);
            }
            else
            {
                if (Main.LocalPlayer.buffTime[BuffSlot] / 60 > 60)
                    buffTimer.SetText(((BuffTime / 60) / 60).ToString() + "m", TextScale, false);
                else
                    buffTimer.SetText((BuffTime / 60).ToString() + "s", TextScale, false);
            }
        }

        public void UpdateImage()
        {
            if (BuffType != 0)
                buffImage.SetImage(GetBuffImage);
            else
                buffImage.SetImage(ModContent.Request<Texture2D>("TerraLeague/Textures/UI/BlankBuff").Value);
        }

        void UpdateScaling()
        {
            Width.Set(ElementWidth, 0);
            Height.Set(ElementHeight, 0);

            buffImage.Width.Set(ImageDimentions, 0);
            buffImage.Height.Set(ImageDimentions, 0);
            buffImage.Left.Set(ElementMargin, 0);
            buffImage.Top.Set(ElementMargin, 0);
            buffImage.ScaleToFit = true;
            buffImage.Color = !BuffTransparent ? Color.White : new Color(100, 100, 100, 100);

            buffTimer.Width.Set(ElementWidth, 0f);
            buffTimer.Height.Set(TextHeight, 0f);
            buffTimer.HAlign = 0.5f;
            //buffTimer.Left.Set(ElementMargin - TextLeftOffset, 0);
            buffTimer.Left.Set(0, 0);
            buffTimer.Top.Set(buffImage.Top.Pixels + ImageTextSpacing + buffImage.Height.Pixels, 0f);
            buffTimer.TextColor = !BuffTransparent ? Color.White : new Color(200, 200, 200, 100);

            Recalculate();
        }

        void SetTooltip()
        {
            if (IsMouseHovering)
            {
                string buffDescription = "";

                ModBuff buffType = ModContent.GetModBuff(BuffType);
                if (buffType != null)
                {
                    string tip = Lang.GetBuffDescription(BuffType);
                    int num = 0;
                    buffType.ModifyBuffTip(ref tip, ref num);
                    buffDescription = LeagueTooltip.CreateColorString(TerraLeague.TooltipHeadingColor, Lang.GetBuffName(BuffType)) +
                        "\n" + tip;
                }
                else
                {
                    buffDescription = LeagueTooltip.CreateColorString(TerraLeague.TooltipHeadingColor, Lang.GetBuffName(BuffType));
                    
                    if (BuffType == Terraria.ID.BuffID.MonsterBanner)
                    {
                        string name1 = "";
                        string name2 = "";
                        string name3 = "";

                        for (int i = 0; i < NPCLoader.NPCCount; i++)
                        {
                            if (Item.BannerToNPC(i) != 0 && Main.LocalPlayer.HasNPCBannerBuff(i))
                            {
                                if (name1 == "")
                                    name1 = LeagueTooltip.CreateColorString(Color.LightGreen, Lang.GetNPCNameValue(Item.BannerToNPC(i)));
                                else if (name2 == "")
                                    name2 = LeagueTooltip.CreateColorString(Color.LightGreen, Lang.GetNPCNameValue(Item.BannerToNPC(i)));
                                else if (name3 == "")
                                    name3 = LeagueTooltip.CreateColorString(Color.LightGreen, Lang.GetNPCNameValue(Item.BannerToNPC(i)));
                                else
                                {
                                    string name4 = LeagueTooltip.CreateColorString(Color.LightGreen, Lang.GetNPCNameValue(Item.BannerToNPC(i)));

                                    int spaces = 18 - name1.Length;

                                    buffDescription += "\n" + name1 + " ~ " + name2 + " ~ " + name3 + " ~ " + name4;

                                    name1 = "";
                                    name2 = "";
                                    name3 = "";
                                }
                            }
                        }

                        if (name1 != "")
                            buffDescription += "\n" + name1;
                        if (name2 != "")
                            buffDescription += " ~ " + name2;
                        if (name3 != "")
                            buffDescription += " ~ " + name3;
                    }
                    else if (BuffType == Terraria.ID.BuffID.ManaSickness)
                    {
                        buffDescription += "\nHeal Power nullified" +
                            "\nMagic and Summon damage reduced by ";
                        buffDescription += (int)(Main.LocalPlayer.manaSickReduction * 100) + "%";
                    }

                    //buffText.SetText(buffDescription);
                }

                if (Lang.GetBuffName(BuffType) != "")
                {
                    Main.mouseText = true;
                    Main.LocalPlayer.cursorItemIconText = buffDescription;
                    ToolTipUI.SetText(buffDescription.Split('\n'));
                }
            }
        }

        public override void RightClick(UIMouseEvent evt)
        {
            Main.LocalPlayer.ClearBuff(BuffType);
        }
    }

    public enum BuffUIDim
    {
        _1x24,
        _2x12,
        _3x8,
        _4x6,
        _6x4,
        _8x3,
        _12x2,
        _24x1,
    }
}
