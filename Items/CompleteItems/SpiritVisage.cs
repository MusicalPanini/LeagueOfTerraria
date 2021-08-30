using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class SpiritVisage : MasterworkItem
    {
        public override string MasterworkName => "Anima Visage";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spirit Visage");
            Tooltip.SetDefault("Increases maximum life by 30" +
                "\nIncreases resist by 6" +
                "\nIncreases life regeneration by 2" +
                "\nIncreases ability haste by 10");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 45, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
            Item.material = true;

            Passives = new Passive[]
            {
                new SpiritualRestoration(this)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 30;
            player.lifeRegen += 2;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 6;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Kindlegem>(), 1)
            .AddIngredient(ItemType<Cowl>(), 1)
            .AddIngredient(ItemID.ChlorophytePlateMail, 1)
            .AddIngredient(ItemType<DamnedSoul>(), 50)
            .AddIngredient(ItemID.ChlorophyteBar, 6)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string MasterworkTooltip()
        {
            return "Increases maximum life by " + LeagueTooltip.CreateColorString(MasterColor, "40") +
                "\nIncreases resist by " + LeagueTooltip.CreateColorString(MasterColor, "8") +
                "\nIncreases life regeneration by " + LeagueTooltip.CreateColorString(MasterColor, "4") +
                "\nIncreases ability haste by " + LeagueTooltip.CreateColorString(MasterColor, "20");
        }

        public override void UpdateMasterwork(Player player)
        {
            player.statLifeMax2 += 10;
            player.lifeRegen += 1;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 2;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
        }
    }
}
