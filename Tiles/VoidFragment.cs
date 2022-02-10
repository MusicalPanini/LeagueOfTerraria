using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace TerraLeague.Tiles
{
    public class VoidFragment : ModTile
    {
        bool pulse = false;
        float bLast = 0.3f;
        public override void SetStaticDefaults()
        {
            SoundType = SoundID.Tink;
            Main.tileSolid[Type] = true; 
            Main.tileMergeDirt[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileBlockLight[Type] = true; 
            
            DustType = DustID.DemonTorch;
            ItemDrop = ItemType<Items.VoidFragment>(); 
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Void Matter");
            AddMapEntry(new Color(200, 0, 200), name); 
            MinPick = 65;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D GlowMask = null;
            TerraLeague.GetTextureIfNull(ref GlowMask, "TerraLeague/Tiles/VoidFragment_Glow");

            Tile tile = Main.tile[i, j];

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            float r = bLast;
            float g = 0.0f;
            float bl = bLast;

            if (pulse)
            {
                bl += 0.000003f;
            }
            else
            {
                bl -= 0.000003f;
            }

            if (bl <= 0.1)
                pulse = true;
            else if (bl >= 0.25)
                pulse = false;
            bLast = bl;
            Color color = new Color(255 * bl, 255 * bl, 255 * bl);

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

            base.PostDraw(i, j, spriteBatch);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            //DustType = DustID.DemonTorch;
            //r = bLast;
            //g = 0.0f;
            //b = bLast;

            //if (pulse)
            //{
            //    b += 0.000003f;
            //}
            //else
            //{
            //    b -= 0.000003f;
            //}

            //if (b <= 0.1)
            //    pulse = true;
            //else if (b >= 0.25)
            //    pulse = false;
            //bLast = b;
        }
    }
}