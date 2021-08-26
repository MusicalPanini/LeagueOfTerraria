using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class VoidbornBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Voidborn Chestpiece");
            Tooltip.SetDefault("8% increased magic and summon damage");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.value = 40000;
            Item.rare = ItemRarityID.Orange;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Magic) += 0.08f;
            player.GetDamage(DamageClass.Summon) += 0.08f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.FossilShirt, 1)
            .AddIngredient(ItemType<VoidFragment>(), 80)
            .AddTile(TileID.Anvils)
            .Register();
            
        }

        public override void UpdateArmorSet(Player player)
        {
            
        }
    }
}
