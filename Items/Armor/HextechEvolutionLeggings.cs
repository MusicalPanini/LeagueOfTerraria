using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class HextechEvolutionLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Hextech Evolution Boots");
            Tooltip.SetDefault("8% reduced mana cost");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = 250000;
            Item.rare = ItemRarityID.Pink;
            Item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            player.manaCost -= 0.08f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup("TerraLeague:Tier3Bar", 16)
            .AddIngredient(ItemType<PerfectHexCore>(), 1)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override void UpdateArmorSet(Player player)
        {
        }
    }
}
