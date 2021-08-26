using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class CrystallineBracer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystalline Bracer");
            Tooltip.SetDefault("Increases maximum life by 20" +
                "\nIncreases life regeneration by 2");
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
            player.lifeRegen += 2;
            player.statLifeMax2 += 20;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<RubyCrystal>(), 1)
            .AddIngredient(ItemType<RejuvBead>(), 1)
            .AddIngredient(ItemID.MeteoriteBar, 5)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
