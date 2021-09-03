using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class IonicSpark : MasterworkItem
    {
        public override string MasterworkName => "Covalent Spark";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ionic Spark");
            Tooltip.SetDefault("12% increased ranged attack speed" +
                "\nIncreases maximum life by 10");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new Energized(10, 20, this),
                new Discharge(this)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 10;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.12;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<RecurveBow>(), 1)
            .AddIngredient(ItemType<KircheisShard>(), 1)
            .AddIngredient(ItemType<RubyCrystal>(), 1)
            .AddRecipeGroup("TerraLeague:IronGroup", 5)
            .AddIngredient(ItemID.Wire, 10)
            .AddTile(TileID.Anvils)
            .Register();
            
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
                return ((int)Passives[0].passiveStat).ToString() + "%";
            else
                return "";
        }

        public override bool OnCooldown(Player player)
        {
            return !Passives[0].currentlyActive;
        }

        public override string MasterworkTooltip()
        {
           return LeagueTooltip.CreateColorString(MasterColor, "20%") + " increased ranged attack speed" +
                "\nIncreases maximum life by " + LeagueTooltip.CreateColorString(MasterColor, "25");
        }

        public override void UpdateMasterwork(Player player)
        {
            player.statLifeMax2 += 15;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.08;
        }
    }
}
