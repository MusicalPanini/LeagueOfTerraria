using Terraria.WorldBuilding;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Tiles
{
    public class PetrifiedGrass : ModTile
    {
        public override void SetStaticDefaults()
        {
            SetModTree(new PetTree());
            AddMapEntry(new Color(191, 191, 191));

            Main.tileSolid[Type] = true;
            Main.tileBlendAll[this.Type] = true;
            //Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            SetModTree(new PetTree());
            //Main.tileMerge[Type][TileID.Grass] = true;
            TileID.Sets.Conversion.Grass[Type] = true;
            TileID.Sets.Conversion.MergesWithDirtInASpecialWay[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            TileID.Sets.ForcedDirtMerging[Type] = true;
            TileID.Sets.CanBeDugByShovel[Type] = true;
            TileID.Sets.ResetsHalfBrickPlacementAttempt[Type] = true;
            TileID.Sets.DoesntPlaceWithTileReplacement[Type] = true;
            TileID.Sets.ChecksForMerge[Type] = true;
            TileID.Sets.SpreadOverground[Type] = true;
            TileID.Sets.SpreadUnderground[Type] = true;

            //TileID.Sets.NeedsGrassFramingDirt[TileID.Marble] = Type;
            //TileID.Sets.NeedsGrassFramingDirt[Type] = TileID.Marble;
            TileID.Sets.NeedsGrassFraming[Type] = true;

            ItemDrop = ItemID.DirtBlock;
        }

        public override void RandomUpdate(int i, int j)
        {
            if (!Framing.GetTileSafely(i, j - 1).IsActive && Main.rand.Next(20) == 0)
            {
                int style = Main.rand.Next(22);
                if (WorldGen.PlaceObject(i, j - 1, TileType<PetrifiedFlora>(), false, style))
                    NetMessage.SendObjectPlacment(-1, i, j - 1, TileType<PetrifiedFlora>(), style, 0, -1, -1);
            }

            if (true /*Main.rand.Next(1) == 0*/)
            {
                for (int x = i - 1; x <= i + 1; x++)
                {
                    for (int y = j - 1; y <= j + 1; y++)
                    {
                        if ((x != i || j != y) && Main.tile[x, y].IsActive && Main.tile[x, y].type == TileID.Dirt)
                        {
                            WorldGen.SpreadGrass(x, y, TileID.Dirt, Type, false, Main.tile[i, j].Color);
                            if ((int)Main.tile[x, y].type == Type)
                            {
                                WorldGen.SquareTileFrame(x, y, true);
                            }
                        }
                    }
                }
            }
            
            base.RandomUpdate(i, j);
        }

        public override int SaplingGrowthType(ref int style)
        {
            style = 0;
            return TileType<PetTreeSapling>();
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!effectOnly)
            {
                fail = true;
                Main.tile[i, j].type = TileID.Dirt;
                WorldGen.SquareTileFrame(i, j, true);
            }
        }
    }
}