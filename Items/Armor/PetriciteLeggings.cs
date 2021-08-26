using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class PetriciteLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Silver-Steel Greaves");
            Tooltip.SetDefault("4 resist" +
                "\nIncreased melee knockback" +
                "\nEnemies are more likely to target you");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = 45000;
            Item.rare = ItemRarityID.Orange;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().resist += 4;
            player.aggro += 150;
            player.kbGlove = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<SilversteelBar>(), 12)
            .AddTile(TileID.Anvils)
            .Register();
            
        }

        public override void UpdateArmorSet(Player player)
        {
        }
    }
}
