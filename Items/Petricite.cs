using TerraLeague.Items.PetrifiedWood;
using TerraLeague.Items.Placeable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class Petricite : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Petricite Slab");
            base.SetStaticDefaults();

            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 99;
            Item.width = 30;
            Item.height = 24;
            Item.uniqueStack = false;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 0, 15, 0);
            Item.createTile = TileType<Tiles.PetriciteBarTile>();
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(4)
            .AddIngredient(ItemType<PetWood>(), 32)
            .AddIngredient(ItemType<Limestone>(), 16)
            .AddIngredient(ItemID.AshBlock, 16)
            .AddTile(TileID.Furnaces)
            .Register();
        }
    }
}
