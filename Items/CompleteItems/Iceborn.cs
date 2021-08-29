using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Iceborn : MasterworkItem
    {
        public override string MasterworkName => "Frozen Fist";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Iceborn Gauntlet");
            Tooltip.SetDefault("Increases armor by 6" +
                "\nIncreases maximum mana by 40" +
                "\nIncreases ability haste by 15" +
                "\nIncreases melee knockback" +
                "\nGrants immunity to knockback");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 45, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new Spellblade(1.5),
                new IcyZone()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 6;
            player.statManaMax2 += 40;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 15;
            player.noKnockback = true;
            player.kbGlove = true;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Sheen>(), 1)
            .AddIngredient(ItemType<GlacialShroud>(), 1)
            .AddIngredient(ItemID.TitanGlove, 1)
            .AddIngredient(ItemType<TrueIceChunk>(), 20)
            .AddIngredient(ItemID.SoulofMight, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string MasterworkTooltip()
        {
            return "Increases maximum mana by " + LeagueTooltip.CreateColorString(MasterColor, "50") +
                "\nIncreases armor by " + LeagueTooltip.CreateColorString(MasterColor, "10") +
                "\nIncreases ability haste by " + LeagueTooltip.CreateColorString(MasterColor, "25") +
                "\nIncreases melee knockback" +
                "\nGrants immunity to knockback";
        }

        public override void UpdateMasterwork(Player player)
        {
            player.statManaMax2 += 10;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 4;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
        }
    }
}
