using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class VoidProphetHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Prophet's Hood");
            Tooltip.SetDefault("Increases your max number of minions" +
            "\nIncreases your max life by 50");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 26;
            Item.value = 300000;
            Item.rare = ItemRarityID.Lime;
            Item.defense = 15;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 1;
            player.statLifeMax2 += 50;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(GetInstance<VoidBar>(), 12)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (body.type == ItemType<VoidProphetGarb>() && legs.type == ItemType<VoidProphetPants>());
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Periodically spawn " + LeagueTooltip.TooltipValue(0, false, "", new System.Tuple<int, ScaleType>(100, ScaleType.Minions)) + " Zz'rots";
            player.armorEffectDrawShadowBasilisk = true;
            player.GetModPlayer<PLAYERGLOBAL>().prophetSet = true;
        }
    }
}
