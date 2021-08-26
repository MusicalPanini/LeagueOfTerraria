using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Stinger : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stinger");
            Tooltip.SetDefault("15% increased melee speed" +
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
            player.meleeSpeed += 0.15f;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.FeralClaws, 1)
            .AddIngredient(ItemType<Dagger>(), 2)
            .AddIngredient(ItemID.Stinger, 5)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
