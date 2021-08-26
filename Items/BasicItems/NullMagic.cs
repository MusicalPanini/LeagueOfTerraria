using TerraLeague.Items.PetrifiedWood;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.BasicItems
{
    public class NullMagic : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Null-Magic Mantle");
            Tooltip.SetDefault("Increases resist by 3");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 2, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
            Item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().resist += 3;

        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Silk, 5)
            .AddIngredient(ItemID.FallenStar, 2)
            .AddIngredient(ItemType<PetWood>(), 10)
            .AddTile(TileID.Loom)
            .Register();
            
        }
    }
}
