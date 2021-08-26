using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class AzirLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Sunstone Greaves");
            Tooltip.SetDefault("Increases maximum mana by 20" +
                "\n10% increased magic damage");
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
            player.statManaMax2 += 20;
            player.GetDamage(DamageClass.Magic) += 0.05f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Sunstone>(), 10)
            .AddIngredient(ItemID.GoldBar, 16)
            .AddTile(TileID.Anvils)
            .Register();
            
        }

        public override void UpdateArmorSet(Player player)
        {
        }
    }
}
