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
    public class HoldoutOriginFix : PlayerDrawLayer
    {
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return 0 != drawInfo.drawPlayer.itemTime;
        }

        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.HeldItem);

        protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Item heldItem = drawInfo.heldItem;
			int num = heldItem.type;
			if (num == ModContent.ItemType<Items.Weapons.MercuryCannon>())
			{
				float adjustedItemScale = drawInfo.drawPlayer.GetAdjustedItemScale(heldItem);
				Main.instance.LoadItem(num);
				Texture2D value = TextureAssets.Item[num].Value;
				float num18 = drawInfo.drawPlayer.itemRotation + 0.785f * (float)drawInfo.drawPlayer.direction;
				float num2 = 0f;
				float num3 = 0f;
				Vector2 origin5 = new Vector2(0f, (float)value.Height);

				Rectangle? sourceRect = new Rectangle(0, 0, value.Width, value.Width);

				if (drawInfo.drawPlayer.gravDir == -1f)
				{
					if (drawInfo.drawPlayer.direction == -1)
					{
						num18 += 1.57f;
						origin5 = new Vector2((float)value.Width, 0f);
						num2 -= (float)value.Width;
					}
					else
					{
						num18 -= 1.57f;
						origin5 = Vector2.Zero;
					}
				}
				else if (drawInfo.drawPlayer.direction == -1)
				{
					origin5 = new Vector2((float)value.Width, (float)value.Height);
					num2 -= (float)value.Width/2;
				}

				ItemLoader.HoldoutOrigin(drawInfo.drawPlayer, ref origin5);
				DrawData item = new DrawData(value, new Vector2((float)(int)(drawInfo.ItemLocation.X - Main.screenPosition.X /*+ origin5.X+ num2*/ ), (float)(int)(drawInfo.ItemLocation.Y - Main.screenPosition.Y + num3)), sourceRect, heldItem.GetAlpha(drawInfo.itemColor), num18, origin5, adjustedItemScale, drawInfo.itemEffect, 0);

				drawInfo.DrawDataCache.RemoveAt(drawInfo.DrawDataCache.Count - 1);
				drawInfo.DrawDataCache.Add(item);
			}
        }
	}
}
