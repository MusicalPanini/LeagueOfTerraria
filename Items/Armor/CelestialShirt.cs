using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class CelestialShirt : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Celestial Garb");
            Tooltip.SetDefault("MEL, RNG, MAG, and SUM increased by 25");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 26;
            Item.value = 9000 * 5;
            Item.rare = ItemRarityID.Green;
            Item.defense = 4;
        }
        
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().BonusMEL += 25;
            player.GetModPlayer<PLAYERGLOBAL>().BonusRNG += 25;
            player.GetModPlayer<PLAYERGLOBAL>().BonusMAG += 25;
            player.GetModPlayer<PLAYERGLOBAL>().BonusSUM += 25;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<CelestialBar>(), 20)
                .AddIngredient(ItemID.Silk, 5)
                .AddIngredient(ItemID.Leather, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
