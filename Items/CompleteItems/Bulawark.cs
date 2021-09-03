using Microsoft.Xna.Framework;
using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Bulawark : MasterworkItem
    {
        public override string MasterworkName => "Bulawark of the Mountain";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Face of the Mountain");
            Tooltip.SetDefault("Increases maximum life by 20" +
                "\nIncreases life regeneration by 2" +
                "\nIncreases ability haste by 15" +
                "\nGrants immunity to knockback");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;

            Active = new DeadlyPhalanx(8, 20, 700, 75, Color.CornflowerBlue);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 20;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            player.lifeRegen += 2;
            player.noKnockback = true;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Kindlegem>(), 1)
            .AddIngredient(ItemType<CrystallineBracer>(), 1)
            .AddIngredient(ItemType<CelestialBar>(), 6)
            .AddIngredient(ItemID.CobaltShield, 1)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string MasterworkTooltip()
        {
            return "Increases maximum life by " + LeagueTooltip.CreateColorString(MasterColor, "30") +
                "\nIncreases life regeneration by " + LeagueTooltip.CreateColorString(MasterColor, "4") +
                "\nIncreases ability haste by " + LeagueTooltip.CreateColorString(MasterColor, "25") +
                "\nGrants immunity to knockback";
        }

        public override void UpdateMasterwork(Player player)
        {
            player.statLifeMax2 += 10;
            player.lifeRegen += 2;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
        }
    }
}
