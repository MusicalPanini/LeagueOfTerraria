using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class HextechKey : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech Key");
            Tooltip.SetDefault("Used to open Hextech Chests");
            base.SetStaticDefaults();

            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.width = 16;
            Item.height = 30;
            Item.rare = ItemRarityID.LightRed;
            Item.value = 30000;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<HextechKeyFragment>(), 3)
            .Register();
            
        }
    }
}
