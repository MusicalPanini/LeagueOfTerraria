using TerraLeague.Items.Placeable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class DarksteelBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darksteel Bar");
            base.SetStaticDefaults();

            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 99;
            Item.width = 30;
            Item.height = 24;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 1, 50, 0);
            Item.uniqueStack = false;
            Item.createTile = TileType<Tiles.DarksteelBarTile>();
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            
        }

        public override void AddRecipes()
        {
            CreateRecipe(2)
            .AddRecipeGroup("TerraLeague:IronGroup", 2)
            .AddIngredient(ItemType<Ferrospike>(), 12)
            .AddTile(TileID.Hellforge)
            .Register();
        }
    }
}
