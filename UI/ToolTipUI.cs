using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System;
using Terraria.ID;
using TerraLeague.Items.SummonerSpells;
using TerraLeague.Items.CustomItems;
using Terraria.UI.Chat;
using Terraria.GameContent;

namespace TerraLeague.UI
{
    public class ToolTipUI : UIState
    {
        public static bool visible { get { return TooltipPanel.tooltip != ""; } }

        //public static int MaxLines = 16;
        //public int itemType;
        //public bool drawText = false;
        static ToolTipPanel TooltipPanel;

        //readonly UIToolTipLine[] uiLines = new UIToolTipLine[MaxLines];
        //readonly string[] tooltip = new string[16];

        public override void OnInitialize()
        {
            TooltipPanel = new ToolTipPanel();
            Append(TooltipPanel);

            base.OnInitialize();
        }

        public static void SetText(string rawTooltip)
        {
            TooltipPanel.tooltip = rawTooltip;
        }

        public static void SetText(string[] tooltipContents)
        {
            string tooltip = tooltipContents[0];

            for (int i = 1; i < tooltipContents.Length; i++)
            {
                tooltip += "\n" + tooltipContents[i] ;
            }

            TooltipPanel.tooltip = tooltip;
        }
    }

    public class ToolTipPanel : UIPanel
    {
        public string tooltip = "";
        UIText textUI;

        public override void OnInitialize()
        {
            BackgroundColor = Color.DarkGreen * 0.75f;

            textUI = new UIText("");
            Append(textUI);

            base.OnInitialize();
        }

        public override void Update(GameTime gameTime)
        {
            BackgroundColor = new Color(0, 50, 25) * 0.9f;
            textUI.SetText(tooltip);
            SetDimentions();
            SetPosition();

            tooltip = "";

            Recalculate();
            base.Update(gameTime);
        }

        public void SetPosition()
        {
            Vector2 position = Main.MouseScreen + new Vector2(10, -10 - Height.Pixels);

            if (position.X < 0)
                position.X = 0;
            if (position.X + Width.Pixels > Main.screenWidth)
                position.X = Main.screenWidth - Width.Pixels;
            if (position.Y < 0)
                position.Y = 0;
            if (position.Y + Height.Pixels > Main.screenHeight)
                position.Y = Main.screenHeight - Height.Pixels;

            Left.Set(position.X, 0);
            Top.Set(position.Y, 0);
        }

        public void SetDimentions()
        {
            Vector2 dimentions = Vector2.Zero;

            string[] lines = tooltip.Split('\n');
            int yOffset = 0;
            for (int k = 0; k < lines.Length; k++)
            {
                Vector2 stringSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, lines[k], Vector2.One);
                if (stringSize.X > dimentions.X)
                {
                    dimentions.X = stringSize.X;
                }
                dimentions.Y += stringSize.Y + (float)yOffset;
            }

            Width.Set(dimentions.X + 22, 0);
            Height.Set(dimentions.Y + 16, 0);
        }
    }

    class UIToolTipLine : UIText
    {
        public UIToolTipLine(int left, int top, string text, float textScale = 1, bool large = false) : base(text, textScale, large)
        {
            SetText(text,textScale, large);
            Left.Set(left, 0f);
            Top.Set(top, 0f);
            Width.Set(500, 0f);
            Height.Set(28, 0);
        }
    }
}