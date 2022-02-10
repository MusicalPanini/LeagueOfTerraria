using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using TerraLeague.Items.Placeable;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;

namespace TerraLeague.Tiles
{
    public class VoidStone : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true; 
            Main.tileLighted[Type] = true; 
            Main.tileBlockLight[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileMerge[Type][TileType<VoidFragment>()] = true;
            Main.tileMerge[TileType<VoidFragment>()][Type] = true;

            SoundType = SoundID.Tink;
            DustType = DustID.Stone;
            ItemDrop = ItemType<TargonStoneBlock>(); 
            AddMapEntry(new Color(100, 0, 100)); 
            MinPick = 65; 
        }

        public override void RandomUpdate(int i, int j)
        {
            if (10 > WorldGen.CountNearBlocksTypes(i, j, 20, 10, TileType<CrystalBomb>()))
            {
                if (!Framing.GetTileSafely(i, j - 1).HasTile && Main.rand.Next(20) == 0)
                {
                    if (WorldGen.PlaceObject(i, j - 1, TileType<CrystalBomb>(), false))
                        NetMessage.SendObjectPlacment(-1, i, j - 1, TileType<CrystalBomb>(), 0, 0, -1, -1);
                }
            }

            base.RandomUpdate(i, j);
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D GlowMask = null;
            TerraLeague.GetTextureIfNull(ref GlowMask, "TerraLeague/Tiles/VoidStone_Glow");

            Tile tile = Main.tile[i, j];

            Color color = Lighting.GetColor(i, j) * 2;

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            if (tile.Slope == SlopeType.Solid && !tile.IsHalfBlock)
            {
                spriteBatch.Draw(
                    GlowMask,
                    new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
                    new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
                    color,
                    0,
                    default,
                    1,
                    SpriteEffects.None,
                    0f
                    );
            }
            else if (tile.IsHalfBlock)
            {
                Main.spriteBatch.Draw(
                    GlowMask,
                    new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 10) + zero,
                    new Rectangle(tile.TileFrameX /*+ drawData.addFrX*/, tile.TileFrameY /*+ drawData.addFrY*/, 16, 6),
                    color,
                    0f,
                    Vector2.Zero,
                    1f,
                    SpriteEffects.None,
                    0f);
            }
            else
            {
                SlopeType b = tile.Slope;
                for (int s = 0; s < 8; s++)
                {
                    int num11 = s << 1;
                    Rectangle value = new Rectangle(tile.TileFrameX, tile.TileFrameY + s * 2, num11, 2);
                    int num12 = 0;
                    switch ((byte)tile.Slope)
                    {
                        case 2:
                            value.X = 16 - num11;
                            num12 = 16 - num11;
                            break;
                        case 3:
                            value.Width = 16 - num11;
                            break;
                        case 4:
                            value.Width = 14 - num11;
                            value.X = num11 + 2;
                            num12 = num11 + 2;
                            break;
                    }
                    Main.spriteBatch.Draw(GlowMask, new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + (float)num12, (float)(j * 16 - (int)Main.screenPosition.Y + s * 2)) + zero, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
            }
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
        }
    }
}