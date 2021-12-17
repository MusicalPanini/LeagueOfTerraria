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
    public class VoidUI : UIState
    {
        public static bool visible { get { return Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().VoidInflu > 0; } }

        static VoidPanel voidPanel;

        public override void OnInitialize()
        {
            voidPanel = new VoidPanel();
            Append(voidPanel);

            base.OnInitialize();
        }
    }

    public class VoidPanel : UIMoveable
    {
        public override UIAnchor Anchor => UIAnchor.TopCenter;
        public override ref int GetXOffset => ref XOffset;
        public override ref int GetYOffset => ref YOffset;
        int XOffset = 0;
        int YOffset = 64;

        VoidInfluBar bar;

        public override void OnInitialize()
        {
            Width.Set(720, 0);
            Height.Set(20, 0f);

            bar = new VoidInfluBar(20, 720);
            // hp.Left.Set(10, 0f);
           // hp.Top.Set(4f, 0f);
            Append(bar);

            base.OnInitialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }

    class UIVoidBar : UIElement
    {

        public Color backgroundColor = Color.Gray;
        private Texture2D _backgroundTexture;

        public UIVoidBar()
        {
            
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            TerraLeague.GetTextureIfNull(ref _backgroundTexture, "TerraLeague/Textures/UI/VoidBar");

            CalculatedStyle dimensions = GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            Main.spriteBatch.Draw(_backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), backgroundColor);
        }
    }

    class UIVoidInnerBar : UIElement
    {
        public Color backgroundColor = Color.Gray;
        private Texture2D texture_innerbar;

        public UIVoidInnerBar()
        {
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            TerraLeague.GetTextureIfNull(ref texture_innerbar, "TerraLeague/UI/Blank");

            CalculatedStyle dimensions = GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            Main.spriteBatch.Draw(texture_innerbar, new Rectangle(point1.X, point1.Y, width, height), backgroundColor);
        }
    }

    class VoidInfluBar : UIElement
    {
        private UIVoidBar barBackground;
        private UIVoidInnerBar currentBar;
        private UIText text;
        private readonly float width;
        private readonly float height;

        public override void OnInitialize()
        {
            Height.Set(height, 0f);
            Width.Set(width, 0f);

            barBackground = new UIVoidBar();
            barBackground.Left.Set(0f, 0f);
            barBackground.Top.Set(0f, 0f);
            barBackground.backgroundColor = Color.White;
            barBackground.Width.Set(width, 0f);
            barBackground.Height.Set(height, 0f);

            currentBar = new UIVoidInnerBar();
            currentBar.SetPadding(0);
            currentBar.Left.Set(8f, 0f);
            currentBar.Top.Set(2, 0f);
            currentBar.Width.Set(width, 0f);
            currentBar.Height.Set(height - 4, 0f);
                
            currentBar.backgroundColor = Color.Purple;
            barBackground.Append(currentBar);

            text = new UIText("0|0"); 
            text.Width.Set(width, 0f);
            text.Height.Set(height, 0f);
            text.Top.Set((height / 2 - text.MinHeight.Pixels / 2), 0f);

            barBackground.Append(text);
            base.Append(barBackground);
        }

        public VoidInfluBar(int height, int width)
        {
            this.width = width;
            this.height = height;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            text.Top.Set(0, 0f);
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (modPlayer.VoidInflu >= 90)
            {
                if (Main.timeForVisualEffects % 60 < 30)
                        currentBar.backgroundColor = Color.DarkRed;
                else
                        currentBar.backgroundColor = Color.Purple;
            }
            else
            {
                currentBar.backgroundColor = Color.Purple;
            }

            float quotient = 1;

            if (modPlayer.VoidInflu >= PLAYERGLOBAL.VoidInfluMax)
                quotient = 1;
            else
                quotient = (float)modPlayer.VoidInflu / (float)PLAYERGLOBAL.VoidInfluMax;
            currentBar.Width.Set(quotient * width - 16, 0f);
            Recalculate();

            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            Player player = Main.LocalPlayer; 
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            

            text.SetText("" + (int)(modPlayer.VoidInflu * 10) + " / " + PLAYERGLOBAL.VoidInfluMax * 10);

            if (IsMouseHovering)
            {
                string tooltip = LeagueTooltip.CreateColorString(Color.Purple, "Void Influence");
                tooltip += "\nWhile in the Void you will slowly come under its influence." +
                    "\nThe more Void Matter you are holding, the faster this will occur." +
                    "\nUnder the full effects, you're mind will be lost and you will perish.";
                ToolTipUI.SetText(tooltip.Split('\n'));
            }

            base.Update(gameTime);
        }
    }
}
