using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class SerratedDirk : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Serrated Dirk");
            Tooltip.SetDefault("3% increased melee and ranged damage" +
                "\nIncreases armor penetration by 5");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.03f;
            player.GetDamage(DamageClass.Ranged) += 0.03f;
            player.armorPenetration += 5;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<LongSword>(), 2)
            .AddIngredient(ItemID.SharkToothNecklace, 1)
            .AddIngredient(ItemID.GoldShortsword, 1)
            .AddTile(TileID.Anvils)
            .Register();

            CreateRecipe()
            .AddIngredient(ItemType<LongSword>(), 2)
            .AddIngredient(ItemID.SharkToothNecklace, 1)
            .AddIngredient(ItemID.PlatinumShortsword, 1)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
