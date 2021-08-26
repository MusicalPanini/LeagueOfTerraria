using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Zeal : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zeal");
            Tooltip.SetDefault("8% increased melee and ranged attack speed" +
                "\n4% increased critical strike chance" +
                "\n5% increased movement speed");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetCritChance(DamageClass.Generic) += 4;
            player.meleeSpeed += 0.08f;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.08;
            player.moveSpeed += 0.05f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Dagger>(), 1)
            .AddIngredient(ItemType<BrawlersGlove>(), 1)
            .AddRecipeGroup("TerraLeague:GoldGroup", 3)
            .AddIngredient(ItemID.Lens, 5)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
