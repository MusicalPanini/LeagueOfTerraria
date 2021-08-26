using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class GlacialShroud : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glacial Shroud");
            Tooltip.SetDefault("Increases maximum mana by 20" +
                "\nIncreases armor by 3" +
                "\nIncreases ability haste by 10" +
                "\nGrants immunity to knockback");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
            Item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 3;
            player.statManaMax2 += 20;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            player.noKnockback = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<SapphireCrystal>(), 1)
            .AddIngredient(ItemType<ClothArmor>(), 1)
            .AddIngredient(ItemID.CobaltShield, 1)
            .AddIngredient(ItemType<TrueIceChunk>(), 2)
            .AddIngredient(ItemID.IceBlock, 20)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
