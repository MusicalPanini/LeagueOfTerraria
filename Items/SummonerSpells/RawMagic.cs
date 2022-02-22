using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerraLeague.Common.ItemDropRules;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.SummonerSpells
{
    public class RawMagic : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 12));
            ItemID.Sets.ItemIconPulse[Item.type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            DisplayName.SetDefault("Raw Magic");

            ItemID.Sets.AnimatesAsSoul[Item.type] = true;

            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.material = true;
            Item.rare = ItemRarityID.Green;
            Item.width = 32;
            Item.height = 32;
            base.SetDefaults();
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale);
        }
    }

    public class RawMagicGlobalNPC : GlobalNPC
    {
        public override void ModifyGlobalLoot(GlobalLoot globalLoot)
        {
            RawMagicDropRule keyDropRule = new RawMagicDropRule();
            IItemDropRule conditionalRule = new LeadingConditionRule(keyDropRule);
            IItemDropRule rule = ItemDropRule.Common(ItemType<RawMagic>(), chanceDenominator: 8);

            conditionalRule.OnSuccess(rule);
            globalLoot.Add(conditionalRule);
        }
    }
}
