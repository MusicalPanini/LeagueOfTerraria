using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Talisman : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Talisman of Ascension");
            Tooltip.SetDefault("Increases maximum life by 10" +
                "\nIncreases life regeneration by 2" +
                "\nIncreases mana regeneration by 50%" +
                "\n10% increased healing power" +
                "\nIncreases ability haste by 15");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;

            Active = new FleetFoot(500, 4, 90);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.moveSpeed += 0.05f;
            player.lifeRegen += 2;
            player.statLifeMax2 += 10;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.5;
            player.GetModPlayer<PLAYERGLOBAL>().healPower += 0.1;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 15;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<CrystallineBracer>(), 1)
            .AddIngredient(ItemType<ForbiddenIdol>(), 1)
            .AddIngredient(ItemType<Sunstone>(), 10)
            .AddIngredient(ItemID.SunplateBlock, 20)
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
    }
}
