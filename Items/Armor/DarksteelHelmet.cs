using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class DarksteelHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darksteel Helmet");
            Tooltip.SetDefault("3 armor" +
            "\nIncreases your max life by 25");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 26;
            Item.value = 50000;
            Item.rare = ItemRarityID.Orange;
            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 3;
            player.statLifeMax2 += 25;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<DarksteelBar>(), 15)
            .AddTile(TileID.Anvils)
            .Register();
            
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {

            if(body.type == ItemType<DarksteelBreastplate>() && legs.type == ItemType<DarksteelLeggings>())
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
            player.setBonus = "+12% melee damage";
            player.GetDamage(DamageClass.Melee) += 0.12f;
        }
    }
}
