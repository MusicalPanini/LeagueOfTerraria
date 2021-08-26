using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using TerraLeague.Items.AdvItems;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Zekes : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zeke's Convergence");
            Tooltip.SetDefault("Increases armor by 6" +
                "\nIncreases resist by 4" +
                "\nIncreases maximum mana by 20" +
                "\nIncreases ability haste by 10" +
                "\nIncreases item haste by 10" +
                "\nGrants immunity to knockback and fire blocks");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 45, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;

            Active = new FrostfireCovenant(40, 10, 90);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 6;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 4;
            player.statManaMax2 += 20;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            player.GetModPlayer<PLAYERGLOBAL>().itemHaste += 10;
            player.noKnockback = true;
            player.fireWalk = true;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Aegis>(), 1)
            .AddIngredient(ItemType<GlacialShroud>(), 1)
            .AddIngredient(ItemID.LivingFireBlock, 10)
            .AddIngredient(ItemType<TrueIceChunk>(), 10)
            .AddIngredient(ItemType<HextechCore>(), 1)
            .AddIngredient(ItemID.SoulofSight, 12)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string GetStatText()
        {
            if (Active.currentlyActive)
            {
                if (Active.cooldownCount > 0)
                    return (Active.cooldownCount / 60).ToString();
            }
            return "";
        }

        public override bool OnCooldown(Player player)
        {
            if (Active.cooldownCount > 0 || !Active.currentlyActive)
                return true;
            else
                return false;
        }
    }
}
