using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;
using TerraLeague.Items.PetrifiedWood;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace TerraLeague.Tiles
{
    public class CrystalBomb : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileCut[Type] = true;
            Main.tileSolid[Type] = true;
            TileID.Sets.Boulders[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Unstable Void Crystal");
            AddMapEntry(new Color(200, 0, 200), name);
            TileID.Sets.DisableSmartCursor[Type] = true;
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            return base.TileFrame(i, j, ref resetFrame, ref noBreak);
        }

        public override bool Dangersense(int i, int j, Player player)
        {
            return true;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Projectile.NewProjectile(new ProjectileSource_TileBreak(i, j), new Vector2((i + 1) * 16, (j + 1) * 16), Vector2.Zero, ProjectileType<Projectiles.VoidbornSlime_CrystalBomb>(), 50, 0, Main.myPlayer);
        }

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            
            Lighting.AddLight(new Vector2((i + 1) * 16, (j + 1) * 16), (Color.Purple * 0.3f).ToVector3());
        }
    }
}