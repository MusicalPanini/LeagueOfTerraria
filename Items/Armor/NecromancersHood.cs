using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class NecromancersHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Necromancer's Hood");
            Tooltip.SetDefault("Increases your max number of minions by 1");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 26;
            Item.value = 4000 * 5;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<DamnedSoul>(), 20)
            .AddIngredient(ItemID.Silk, 8)
            .AddTile(TileID.Loom)
            .Register();
            
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {

            if(body.type == ItemType<NecromancersRobe>())
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
            player.setBonus = "+10% summon damage";
            player.GetDamage(DamageClass.Summon) += 0.1f;
        }
    }
}
