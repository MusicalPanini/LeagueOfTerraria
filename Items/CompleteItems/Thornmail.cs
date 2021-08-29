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
    public class Thornmail : MasterworkItem
    {
        public override string MasterworkName => "Razormail";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Thornmail");
            Tooltip.SetDefault("Increases maximum life by 20" +
                "\nIncreases armor by 6");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 45, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
            Item.material = true;

            Passives = new Passive[]
            {
                new Thorns(),
                new ColdSteel(2, 350)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 20;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 6;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Bramble>(), 1)
            .AddIngredient(ItemType<RubyCrystal>(), 1)
            .AddIngredient(ItemType<Wardens>(), 1)
            .AddIngredient(ItemID.Spike, 10)
            .AddIngredient(ItemID.TurtleScaleMail, 1)
            .AddIngredient(ItemID.ChlorophyteBar, 6)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string MasterworkTooltip()
        {
            return "Increases maximum life by " + LeagueTooltip.CreateColorString(MasterColor, "30") +
                "\nIncreases armor by " + LeagueTooltip.CreateColorString(MasterColor, "12");
        }

        public override void UpdateMasterwork(Player player)
        {
            player.statLifeMax2 += 10;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 6;
        }
    }
}
