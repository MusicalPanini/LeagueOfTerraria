using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class AzirBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Sunstone Breastplate");
            Tooltip.SetDefault("5% increased summon damage" +
                "\nIncreases your max number of minions by 1");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 28;
            Item.value = 6000 * 5;
            Item.rare = ItemRarityID.Green;
            Item.defense = 4;
        }


        
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon) += 0.05f;
            player.maxMinions += 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<Sunstone>(), 10)
                .AddIngredient(ItemID.GoldBar, 20)
                .AddIngredient(ItemID.Sapphire, 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
