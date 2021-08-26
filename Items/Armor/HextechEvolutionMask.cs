using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class HextechEvolutionMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech Evolution Mask");
            Tooltip.SetDefault("15% increased magic damage" +
                "\nIncreases ability haste by 10");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 24;
            Item.value = 250000;
            Item.rare = ItemRarityID.Pink;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Magic) += 0.15f;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("TerraLeague:Tier3Bar", 10)
                .AddIngredient(ItemType<PerfectHexCore>())
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            if(body.type == ItemType<HextechEvolutionBreastplate>() && legs.type == ItemType<HextechEvolutionLeggings>())
                return true;
            else
                return false;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Periodically fire a lazer at near by enemies";
            player.GetModPlayer<PLAYERGLOBAL>().hextechEvolutionSet = true;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
            //drawAltHair = true;
        }
    }
}
