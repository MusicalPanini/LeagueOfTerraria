using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class VoidHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Warped Helmet");
            Tooltip.SetDefault("Increases your max number of minions by 2" +
            "\n5% increased summon damage");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 16;
            Item.value = 45000;
            Item.rare = ItemRarityID.Orange;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 2;
            player.GetDamage(DamageClass.Summon) += 0.05f;
        }

        public override void AddRecipes()
        {
            
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {

            if(body.type == ItemType<VoidBreastplate>() && legs.type == ItemType<VoidLeggings>())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Increases your max number of minions" +
                "\n5% increased melee damage";
            player.maxMinions += 1;
            player.GetDamage(DamageClass.Melee) += 0.05f;
        }
    }
}
