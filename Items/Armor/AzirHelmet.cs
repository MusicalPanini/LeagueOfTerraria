using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class AzirHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sunstone Helmet");
            Tooltip.SetDefault("5% increased magic and summon damage");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 20;
            Item.value = 6000 * 5;
            Item.rare = ItemRarityID.Green;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon) += 0.05f;
            player.GetDamage(DamageClass.Magic) += 0.05f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Sunstone>(), 10)
            .AddIngredient(ItemID.GoldBar, 10)
            .AddIngredient(ItemID.Sapphire, 2)
            .AddTile(TileID.Anvils)
            .Register();
            
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {

            if(body.type == ItemType<AzirBreastplate>() && legs.type == ItemType<AzirLeggings>())
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
            player.setBonus = "Summon a sand tower that fires at nearby enemies";
        }
    }
}
