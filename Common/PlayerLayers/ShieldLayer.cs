using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TerraLeague.Common.PlayerLayers
{
    public class ShieldLayer : PlayerDrawLayer
    {
        private Asset<Texture2D> shieldTexture;

        public override bool IsHeadLayer => false;

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return drawInfo.drawPlayer.GetModPlayer<PLAYERGLOBAL>().GetTotalShield() > 0;
        }

        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.CaptureTheGem);

        protected override void Draw(ref PlayerDrawSet drawInfo)
		{
            //TerraLeague.GetTextureIfNull(ref shieldTexture, "TerraLeague/Common/PlayerLayers/NormalShield");

			if (shieldTexture == null)
			{
				shieldTexture = ModContent.Request<Texture2D>("TerraLeague/Common/PlayerLayers/NormalShield");
			}

            Player drawPlayer = drawInfo.drawPlayer;
            PLAYERGLOBAL modPlayer = drawPlayer.GetModPlayer<PLAYERGLOBAL>();

            int frame = 0;

            if (modPlayer.shieldFrame < 4)
                frame = 0;
            else if (modPlayer.shieldFrame < 8)
                frame = 1;
            else if (modPlayer.shieldFrame < 12)
                frame = 2;
            else if (modPlayer.shieldFrame < 16)
                frame = 3;
            else if (modPlayer.shieldFrame < 20)
                frame = 4;
            else if (modPlayer.shieldFrame < 24)
                frame = 5;

            if (drawInfo.shadow != 0f)
            {
                return;
            }

            if (modPlayer.currentShieldColor.A != 0 && drawPlayer.active)
            {
                Color color = modPlayer.currentShieldColor;
                color.MultiplyRGB(Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16));
                color.A = 100;
                Rectangle destRec = new Rectangle((int)(drawPlayer.RotatedRelativePoint(drawPlayer.Center).X - Main.screenPosition.X /*- 19 + 30*/), (int)(drawPlayer.RotatedRelativePoint(drawPlayer.Center).Y - Main.screenPosition.Y - 2 /*- 10 + 30*/), 60, 60);

                Lighting.AddLight(drawPlayer.Center, color.ToVector3());

                Texture2D texture = shieldTexture.Value;
                Rectangle sourRec = new Rectangle(0, 0 + (60 * frame), 60, 60);
                DrawData data = new DrawData(texture, destRec, sourRec, color, 0, new Vector2(30, 30), SpriteEffects.None, 1);

                drawInfo.DrawDataCache.Add(new DrawData(
                shieldTexture.Value, //The texture to render.
                new Vector2(destRec.X, destRec.Y), //Position to render at.
                sourRec, //Source rectangle.
                color, //Color.
                0f, //Rotation.
                sourRec.Size() * 0.5f, //Origin. Uses the texture's center.
                1f, //Scale.
                SpriteEffects.None, //SpriteEffects.
                0 //'Layer'. This is always 0 in Terraria.
            ));
            }
        }
	}
}
