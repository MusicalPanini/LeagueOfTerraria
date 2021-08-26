using TerraLeague.Items.Placeable;
using TerraLeague.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class CelestialBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Celestial Bar");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 99;
            Item.width = 30;
            Item.height = 24;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.uniqueStack = false;
            Item.createTile = TileType<CelestialBarTile>();
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
        }

        public override void AddRecipes()
        {
            CreateRecipe(4)
            .AddIngredient(ItemType<TargonGraniteBlock>(), 16)
            .AddIngredient(ItemID.FallenStar, 1)
            .AddTile(TileID.Furnaces)
            .Register();
        }
    }
}
