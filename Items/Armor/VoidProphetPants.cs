using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class VoidProphetPants : ModItem
    {

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Void Prophet's Pants");
            Tooltip.SetDefault("12% increased summon damage" +
                "\nIncreases your max number of sentries");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = 250000;
            Item.rare = ItemRarityID.Lime;
            Item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxTurrets++;
            player.GetDamage(DamageClass.Summon) += 0.12f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<VoidBar>(), 18)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }
    }
}
