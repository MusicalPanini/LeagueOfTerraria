using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class GiantsBelt : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Giant's Belt");
            Tooltip.SetDefault("Increases maximum life by 20");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 7, 50, 0);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
            Item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 20;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<RubyCrystal>(), 1)
            .AddIngredient(ItemID.LifeCrystal, 2)
            .AddIngredient(ItemType<DarksteelBar>(), 8)
            .AddIngredient(ItemType<SilversteelBar>(), 4)
            .AddIngredient(ItemID.Leather, 5)
            .AddIngredient(ItemID.Chain, 4)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
