using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Placeable;
using Terraria.ID;

namespace TerraLeague.Tiles
{
    public class FerrospikeOre : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSpelunker[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true; 
            Main.tileSpelunker[Type] = true;
            Main.tileOreFinderPriority[Type] = 420;
            Main.tileLighted[Type] = true;
            SoundType = SoundID.Tink;
            DustType = DustID.BlueMoss;
            ItemDrop = ItemType<Ferrospike>();
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Ferrospike");
            AddMapEntry(new Color(25, 25, 50), name);
            MinPick = 65; 
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}