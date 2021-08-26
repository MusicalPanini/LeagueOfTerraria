using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class CelestialBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Celestial Boots");
            Tooltip.SetDefault("Increases mana regeneration by 50%" +
                "\n7% increased movement speed");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = 9000 * 5;
            Item.rare = ItemRarityID.Green;
            Item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.07f;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.5;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<CelestialBar>(), 16)
            .AddIngredient(ItemID.Leather, 10)
            .AddTile(TileID.Anvils)
            .Register();
            
        }

        public override void UpdateArmorSet(Player player)
        {
        }
    }
}
