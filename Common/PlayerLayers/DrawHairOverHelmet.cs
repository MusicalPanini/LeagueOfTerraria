using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Common.PlayerLayers
{
    public class DrawHairOverHelmet : PlayerDrawLayer
    {
        public override bool IsHeadLayer => true;

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return !drawInfo.drawPlayer.invis && TerraLeague.DrawHairOverHelmet.Contains(drawInfo.drawPlayer.head) && (drawInfo.drawPlayer.face < 0 || !ArmorIDs.Face.Sets.PreventHairDraw[drawInfo.drawPlayer.face]);
        }

        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

        protected override void Draw(ref PlayerDrawSet drawInfo)
		{
            Vector2 value = new Vector2((float)(-drawInfo.drawPlayer.bodyFrame.Width / 2 + drawInfo.drawPlayer.width / 2), (float)(drawInfo.drawPlayer.height - drawInfo.drawPlayer.bodyFrame.Height + 4));
            Vector2 position = (drawInfo.Position - Main.screenPosition + value).Floor() + drawInfo.drawPlayer.headPosition + drawInfo.headVect;
            if (((Enum)(object)drawInfo.playerEffect).HasFlag((Enum)(object)SpriteEffects.FlipVertically))
            {
                int num = drawInfo.drawPlayer.bodyFrame.Height - drawInfo.hairFrontFrame.Height;
                position.Y += (float)num;
            }
            DrawData item = new DrawData((Texture2D)TextureAssets.PlayerHair[drawInfo.drawPlayer.hair].Value, position, drawInfo.hairFrontFrame, drawInfo.colorHair, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);
            item.shader = drawInfo.hairDyePacked;
            drawInfo.DrawDataCache.Add(item);
        }
	}
}
