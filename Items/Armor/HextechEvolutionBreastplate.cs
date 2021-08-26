using TerraLeague.Items.PetrifiedWood;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class HextechEvolutionBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Hextech Evolution Breastplate");
            Tooltip.SetDefault("Increased maximum mana by 40" +
                "\n5% increased magic damage and critical strike chance");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.value = 250000;
            Item.rare = ItemRarityID.Pink;
            Item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 40;
            player.GetCritChance(DamageClass.Magic) += 5;
            player.GetDamage(DamageClass.Magic) += 0.05f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup("TerraLeague:Tier3Bar", 20)
            .AddIngredient(ItemType<PerfectHexCore>())
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override void UpdateArmorSet(Player player)
        {
        }
    }
}
