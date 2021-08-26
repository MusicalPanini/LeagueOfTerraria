using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class SolariHeadPiece : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solari Head-Piece");
            Tooltip.SetDefault("Increases your max life by 10" +
                "\nIncreases life regeneration by 2");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 26;
            Item.value = 145000 * 5;
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 3;
            player.statLifeMax2 += 25;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<CelestialBar>(), 8)
            .AddIngredient(ItemType<FragmentOfTheAspect>(), 1)
            .AddIngredient(ItemID.LunarTabletFragment, 8)
            .AddTile(TileID.Anvils)
            .Register();
            
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (body.type == ItemType<SolariBreastplate>() && legs.type == ItemType<SolariLeggings>());
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Gain improved stats during the day." +
                "\nCharge up solar energy during the day" +
                "\nAt full charge double tap " + Terraria.Localization.Language.GetTextValue(Main.ReversedUpDownArmorSetBonuses ? "Key.UP" : "Key.DOWN") + " to summon a Solar Flare";
            player.GetModPlayer<PLAYERGLOBAL>().solariSet = true;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
        }
    }
}
