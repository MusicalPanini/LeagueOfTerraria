using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class FrozenHeart : MasterworkItem
    {
        public override string MasterworkName => "Cryogenic Heart";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frozen Heart");
            Tooltip.SetDefault("Increases mana by 40" +
                "\nIncreases armor by 6" +
                 "\nIncreases ability haste by 25" +
                 "\nGrants immunity to knockback");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
            Item.material = true;

            Passives = new Passive[]
            {
                new ColdSteel(3, 400)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statManaMax2 += 40;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 6;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 20;
            player.noKnockback = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Wardens>(), 1)
            .AddIngredient(ItemType<GlacialShroud>(), 1)
            .AddIngredient(ItemID.FrostCore, 1)
            .AddIngredient(ItemType<TrueIceChunk>(), 6)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string MasterworkTooltip()
        {
            return "Increases mana by " + LeagueTooltip.CreateColorString(MasterColor, "60") +
                "\nIncreases armor by " + LeagueTooltip.CreateColorString(MasterColor, "10") +
                 "\nIncreases ability haste by " + LeagueTooltip.CreateColorString(MasterColor, "30") +
                 "\nGrants immunity to knockback";
        }

        public override void UpdateMasterwork(Player player)
        {
            player.statManaMax2 += 20;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 4;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 5;
        }
    }
}
