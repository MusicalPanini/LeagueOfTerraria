using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Kindlegem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kindlegem");
            Tooltip.SetDefault("Increases maximum life by 10" +
                "\nIncreases ability haste by 10");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
            Item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 10;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<RubyCrystal>(), 1)
            .AddIngredient(ItemID.Hellstone, 10)
            .AddIngredient(ItemType<Sunstone>(), 3)
            .AddIngredient(ItemID.Fireblossom, 3)
            .AddTile(TileID.Furnaces)
            .Register();
            
        }
    }
}
