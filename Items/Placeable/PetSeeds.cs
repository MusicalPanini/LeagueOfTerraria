using Microsoft.Xna.Framework;
using TerraLeague.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Placeable
{
    class PetSeeds : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Petrified Seeds");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.value = 75;
            Item.rare = ItemRarityID.White;
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = TileType<PetrifiedGrass>();
        }

        public override bool CanUseItem(Player player)
        {
            int X = Main.MouseWorld.ToTileCoordinates().X;
            int Y = Main.MouseWorld.ToTileCoordinates().Y;

            if (Main.tile[X, Y].type == TileID.Dirt && Main.tile[X, Y].IsActive)
            {
                WorldGen.KillTile(X, Y, false, false, true);
                return base.CanUseItem(player);
            }

            return false;
        }
    }
}
