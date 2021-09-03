using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Redemption : MasterworkItem
    {
        public override string MasterworkName => "Salvation";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Redemption");
            Tooltip.SetDefault("Increases maximum life by 20" +
                "\nIncreases life regeneration by 2" +
                "\nIncreases mana regeneration by 50%" +
                "\n10% increased healing power" +
                "\nIncreases ability haste by 10");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;

            Active = new Intervention(50, 120);
            //Active = new Rejuvenate(50, 500, 120);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().healPower += 0.1;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            player.statLifeMax2 += 20;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.5;
            player.lifeRegen += 2;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<CrystallineBracer>(), 1)
            .AddIngredient(ItemType<ForbiddenIdol>(), 1)
            .AddIngredient(ItemID.LifeCrystal, 2)
            .AddIngredient(ItemID.SoulofLight, 3)
            .AddIngredient(ItemID.HealingPotion, 30)
            .AddTile(TileID.Anvils)
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

        public override string MasterworkTooltip()
        {
            return "Increases maximum life by " + LeagueTooltip.CreateColorString(MasterColor, "25") +
                "\nIncreases life regeneration by " + LeagueTooltip.CreateColorString(MasterColor, "3") +
                "\nIncreases mana regeneration by " + LeagueTooltip.CreateColorString(MasterColor, "75") +
                "\n" + LeagueTooltip.CreateColorString(MasterColor, "20%") + " increased healing power" +
                "\nIncreases ability haste by " + LeagueTooltip.CreateColorString(MasterColor, "15");
        }

        public override void UpdateMasterwork(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().healPower += 0.1;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            player.statLifeMax2 += 5;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.25;
            player.lifeRegen += 3;
        }
    }
}
