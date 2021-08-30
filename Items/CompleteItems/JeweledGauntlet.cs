using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using TerraLeague.Items.AdvItems;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.BasicItems;

namespace TerraLeague.Items.CompleteItems
{
    public class JeweledGauntlet : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jeweled Gauntlet");
            Tooltip.SetDefault("8% increased damage" +
                "\n8% increased crit chance" +
                "\nIncreases maximum mana by 40" +
                "\nIncreases ability haste by 10");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 45, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new ArcanePrecision(this),
                new Haste(this)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Generic) += 0.08f;
            player.GetCritChance(DamageClass.Generic) += 8;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            player.statManaMax2 += 40;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<BFSword>(), 1)
            .AddIngredient(ItemType<BrawlersGlove>(), 1)
            .AddIngredient(ItemType<LostChapter>(), 1)
            .AddIngredient(ItemID.DestroyerEmblem, 1)
            .AddIngredient(ItemID.SoulofMight, 6)
            .AddIngredient(ItemID.HallowedBar, 12)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }
    }
}
