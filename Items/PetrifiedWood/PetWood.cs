using TerraLeague.Items.Placeable;
using TerraLeague.Tiles;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.PetrifiedWood
{
    public class PetWood : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Petrified Wood");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.width = 24;
            Item.height = 22;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = TileType<PetWoodTile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<PetWall>(), 4)
            .AddTile(TileID.WorkBenches)
            .Register();
            
        }
    }
}
