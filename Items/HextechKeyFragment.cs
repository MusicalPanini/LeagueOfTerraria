using TerraLeague.Common.ItemDropRules;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class HextechKeyFragment : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech Key Fragment");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 300;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.width = 12;
            Item.height = 20;
            Item.rare = ItemRarityID.LightRed;
            Item.value = 10000;
        }
    }

    public class KeyFragGlobalNPC : GlobalNPC
    {
        public override void ModifyGlobalLoot(GlobalLoot globalLoot)
        {
            KeyDropRule keyDropRule = new KeyDropRule();
            IItemDropRule conditionalRule = new LeadingConditionRule(keyDropRule);
            IItemDropRule rule = ItemDropRule.Common(ItemType<HextechKeyFragment>(), chanceDenominator: 15);

            conditionalRule.OnSuccess(rule);
            globalLoot.Add(conditionalRule);
        }
    }
}
