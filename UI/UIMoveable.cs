using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace TerraLeague.UI
{
    public abstract class UIMoveable : UIElement
    {
        abstract public UIAnchor Anchor { get; }
        abstract public ref int GetXOffset { get; }
        abstract public ref int GetYOffset { get; }

        public TerraLeagueUIConfig Config;

        public bool moveMode = false;
        float mouseXOri = 0;
        float mouseYOri = 0;
        float uiXOffset = 0;
        float uiYOffset = 0;

        public override void OnInitialize()
        {
            Config = ModContent.GetInstance<TerraLeagueUIConfig>();
            base.OnInitialize();
        }

        public override void Update(GameTime gameTime)
        {
            MoveMode(Anchor, ref GetXOffset, ref GetYOffset);
            Recalculate();
            base.Update(gameTime);
        }

        protected void MoveMode(UIAnchor anchor, ref int UIXOffset, ref int UIYOffset)
        {
            if (moveMode)
            {
                var dimentions = GetDimensions();

                Left.Set(Main.MouseScreen.X - uiXOffset, 0);
                Top.Set(Main.MouseScreen.Y - uiYOffset, 0);
                ApplyOffset(anchor, ref UIXOffset, ref UIYOffset);
            }
            else
            {
                bool somethingChanged = false;
                var dimentions = this.GetDimensions();
                if (dimentions.X < 0)
                {
                    //LeftPercent = 0;
                    Left.Set(0, 0);
                    somethingChanged = true;
                }
                if (dimentions.X + Width.Pixels > Main.screenWidth)
                {
                    //LeftPercent = (Main.screenWidth - Width.Pixels) / (float)Main.screenWidth;
                    Left.Set(Main.screenWidth - Width.Pixels, 0);
                    somethingChanged = true;
                }
                if (dimentions.Y < 0)
                {
                    //TopPercent = 0;
                    Top.Set(0, 0);
                    somethingChanged = true;
                }
                if (dimentions.Y + Height.Pixels > Main.screenHeight)
                {
                    //TopPercent = (Main.screenHeight - Height.Pixels) / (float)Main.screenHeight;
                    Top.Set(Main.screenHeight - Height.Pixels, 0);
                    somethingChanged = true;
                }

                if (somethingChanged)
                    ApplyOffset(anchor, ref GetXOffset, ref GetYOffset);
                else
                    SetPosition(anchor, GetXOffset, GetYOffset, this);
            }
        }

        protected void ApplyOffset(UIAnchor anchor, ref int UIXOffset, ref int UIYOffset)
        {
            Vector2 offset = CreateAnchorOffset(anchor);
            UIXOffset = (int)offset.X;
            UIYOffset = (int)offset.Y;
        }

        public static void SetPosition(UIAnchor anchor, int xOffset, int yOffset, UIMoveable UI)
        {
            // X
            switch (anchor)
            {
                case UIAnchor.TopLeft:
                case UIAnchor.LeftCenter:
                case UIAnchor.BottomLeft:
                    UI.Left.Set(xOffset, 0);
                    break;
                case UIAnchor.TopRight:
                case UIAnchor.RightCenter:
                case UIAnchor.BottomRight:
                    UI.Left.Set(Main.screenWidth - (xOffset + UI.Width.Pixels), 0);
                    break;
                case UIAnchor.TopCenter:
                case UIAnchor.BottomCenter:
                case UIAnchor.Center:
                    UI.Left.Set((Main.screenWidth / 2) + (xOffset - (UI.Width.Pixels / 2)), 0);
                    break;
                default:
                    break;
            }

            // Y
            switch (anchor)
            {
                case UIAnchor.TopLeft:
                case UIAnchor.TopRight:
                case UIAnchor.TopCenter:
                    UI.Top.Set(yOffset, 0);
                    break;
                case UIAnchor.BottomLeft:
                case UIAnchor.BottomRight:
                case UIAnchor.BottomCenter:
                    UI.Top.Set(Main.screenHeight - (yOffset + UI.Height.Pixels), 0);
                    break;
                case UIAnchor.RightCenter:
                case UIAnchor.LeftCenter:
                case UIAnchor.Center:
                    UI.Top.Set((Main.screenHeight / 2) - (UI.Height.Pixels / 2) + yOffset, 0);
                    break;
                default:
                    break;
            }
        }

        protected Vector2 CreateAnchorOffset(UIAnchor anchor)
        {
            if (!TerraLeague.LockUI)
            {
                Vector2 offset = Vector2.Zero;

                // X
                switch (anchor)
                {
                    case UIAnchor.TopLeft:
                    case UIAnchor.LeftCenter:
                    case UIAnchor.BottomLeft:
                        offset.X = Left.Pixels;
                        break;
                    case UIAnchor.TopRight:
                    case UIAnchor.RightCenter:
                    case UIAnchor.BottomRight:
                        offset.X = Main.screenWidth - (Left.Pixels + Width.Pixels);
                        break;
                    case UIAnchor.TopCenter:
                    case UIAnchor.BottomCenter:
                    case UIAnchor.Center:
                        offset.X = (Left.Pixels + (Width.Pixels / 2)) - (Main.screenWidth / 2);
                        break;
                    default:
                        break;
                }

                // Y
                switch (anchor)
                {
                    case UIAnchor.TopLeft:
                    case UIAnchor.TopRight:
                    case UIAnchor.TopCenter:
                        offset.Y = Top.Pixels;
                        break;
                    case UIAnchor.BottomLeft:
                    case UIAnchor.BottomRight:
                    case UIAnchor.BottomCenter:
                        offset.Y = Main.screenHeight - (Top.Pixels + Height.Pixels);
                        break;
                    case UIAnchor.RightCenter:
                    case UIAnchor.LeftCenter:
                    case UIAnchor.Center:
                        offset.Y = (Top.Pixels + (Height.Pixels / 2)) - (Main.screenHeight / 2);
                        break;
                    default:
                        break;
                }

                return offset;
            }
            return new Vector2(GetXOffset, GetYOffset);
        }

        public override void RightMouseDown(UIMouseEvent evt)
        {
            if (!TerraLeague.LockUI)
            {
                moveMode = true;

                mouseXOri = Main.MouseScreen.X;
                mouseYOri = Main.MouseScreen.Y;

                uiXOffset = mouseXOri - Left.Pixels;
                uiYOffset = mouseYOri - Top.Pixels;
            }
            base.RightMouseDown(evt);
        }

        public override void RightMouseUp(UIMouseEvent evt)
        {
            if (!TerraLeague.LockUI)
            {
                moveMode = false;
                ApplyOffset(Anchor, ref GetXOffset, ref GetYOffset);
                //LeftPercent = Left.Percent;
                //TopPercent = Top.Percent;

                base.RightMouseUp(evt);
            }
        }
    }

    public enum UIAnchor
    {
        TopLeft,
        TopCenter,
        TopRight,
        RightCenter,
        BottomRight,
        BottomCenter,
        BottomLeft,
        LeftCenter,
        Center
    }
}
