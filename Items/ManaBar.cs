using TerraLeague.Items.Placeable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class ManaBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mana Infused Bar");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 99;
            Item.width = 30;
            Item.height = 24;
            Item.uniqueStack = false;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 2, 0, 0); 
            Item.createTile = TileType<Tiles.ManaBarTile>();
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
            .AddRecipeGroup("TerraLeague:GoldGroup", 4)
            .AddIngredient(ItemType<ManaStone>(), 16)
            .AddTile(TileID.Hellforge)
            .Register();

        }
    }
}
