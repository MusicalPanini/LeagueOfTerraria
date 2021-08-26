using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class VoidProphetGarb : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Void Prophet's Garb");
            Tooltip.SetDefault("12% increased summon damage" +
                "\nIncreases your max number of minions");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.value = 250000;
            Item.rare = ItemRarityID.Lime;
            Item.defense = 14;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions++;
            player.GetDamage(DamageClass.Summon) += 0.12f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<VoidBar>(), 24)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override void UpdateArmorSet(Player player)
        {
            
        }

        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawHands = false;
            drawArms = false;
        }
    }
}
